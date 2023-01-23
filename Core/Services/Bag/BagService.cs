using System.Collections.Immutable;
using Core.Clients.Vending;
using Core.Extensions;
using Core.Repositories.BagContent;
using Core.Repositories.BagSection;

namespace Core.Services.Bag;

public class BagService : IBagService
{
    private readonly IBagSectionRepository _bagSectionRepository;
    private readonly IBagContentRepository _bagContentRepository;
    private readonly IVendingClient _vendingClient;

    public BagService(IBagSectionRepository bagSectionRepository, IBagContentRepository bagContentRepository,
        IVendingClient vendingClient)
    {
        _bagSectionRepository = bagSectionRepository;
        _bagContentRepository = bagContentRepository;
        _vendingClient = vendingClient;
    }

    public async Task<bool> DecreaseContentCountAsync(Guid contentId)
    {
        var bagContentDetails = await _bagContentRepository.GetOrDefaultAsync(contentId);
        if (bagContentDetails == default)
        {
            return false;
        }

        var updatedCount = bagContentDetails.Count - 1;
        if (updatedCount == 0)
        {
            var isRemoved = await RemoveContentAsync(bagContentDetails.Id);
            return isRemoved;
        }

        var bagContentUpdate = new BagContentUpdate(updatedCount);
        var updateBagContentDetails = await _bagContentRepository.UpdateAsync(bagContentDetails.Id, bagContentUpdate);
        return updateBagContentDetails != default;
    }

    public async Task<IReadOnlyCollection<BagSection>> GetSectionsAsync()
    {
        // get sections data
        var bagSectionsData = _bagSectionRepository.GetAll().ToImmutableArray();

        var isEmpty = !bagSectionsData.Any();
        if (isEmpty)
        {
            return Enumerable.Empty<BagSection>().ToReadOnlyCollection();
        }

        // api request
        var specifications = bagSectionsData.Select(bagSectionData =>
        {
            var itemsIds = bagSectionData.Contents.Select(bagContentData => bagContentData.ItemId);
            var specification = new PickupPointContentsSpecification(bagSectionData.PickupPointId, itemsIds);
            return specification;
        });
        var pickupPointsPresentations = await _vendingClient.GetPickupPointsPresentationsAsync(specifications);

        var bagSections = bagSectionsData.Select(bagSectionData =>
            {
                var bagSection = GetBagSectionFromPickupPointsPresentations(bagSectionData, pickupPointsPresentations);
                return bagSection;
            })
            .OrderBy(m => m.Id)
            .ToReadOnlyCollection();
        return bagSections;
    }

    public async Task<bool> IncreaseContentCountAsync(Guid contentId)
    {
        var bagContentDetails = await _bagContentRepository.GetOrDefaultAsync(contentId);
        if (bagContentDetails == default)
        {
            return false;
        }

        var updatedCount = bagContentDetails.Count + 1;
        var bagContentUpdate = new BagContentUpdate(updatedCount);
        var updateBagContentDetails = await _bagContentRepository.UpdateAsync(bagContentDetails.Id, bagContentUpdate);
        return updateBagContentDetails != default;
    }

    public async Task<bool> RemoveContentAsync(Guid contentId)
    {
        var bagContentDetails = await _bagContentRepository.GetOrDefaultAsync(contentId);
        if (bagContentDetails == default)
        {
            return false;
        }

        var contentSectionId = bagContentDetails.SectionId;
        var bagSectionDetails = await _bagSectionRepository.GetByIdOrDefaultAsync(contentSectionId);
        if (bagSectionDetails == default)
        {
            return false;
        }

        var sectionContentsCount = bagSectionDetails.Contents.Count;
        if (sectionContentsCount == 1)
        {
            var isSectionRemoved = await _bagSectionRepository.DeleteAsync(contentSectionId);
            return isSectionRemoved;
        }

        var isContentRemoved = await _bagContentRepository.DeleteAsync(bagContentDetails.Id);
        return isContentRemoved;
    }

    private static BagSection GetBagSectionFromPickupPointsPresentations(BagSectionDetails bagSectionData,
        IEnumerable<PickupPointPresentation> pickupPointsPresentations)
    {
        // get pickup point contents
        var pickupPointId = bagSectionData.PickupPointId;
        var pickupPointPresentation = pickupPointsPresentations.Single(c => c.PickupPoint.Id == pickupPointId);

        var pickupPoint = MapToPickupPoint(pickupPointPresentation.PickupPoint);
        var contents = bagSectionData.Contents.Select(bagContentData =>
            {
                var bagContent = GetBagContentFromPickupPointContents(bagContentData, pickupPointPresentation.Contents);
                return bagContent;
            })
            .OrderBy(i => i.Id)
            .ToReadOnlyCollection();
        var totalPrice = contents.Sum(content => content.TotalPrice);

        var bagSection = new BagSection(bagSectionData.Id, pickupPoint, contents, totalPrice);
        return bagSection;
    }

    private static BagContent GetBagContentFromPickupPointContents(BagContentBrief bagContentData,
        IEnumerable<ContentDetails> pickupPointContents)
    {
        var itemId = bagContentData.ItemId;
        var (itemDetails, availableCount, price) = pickupPointContents.Single(c => c.Item.Id == itemId);

        var item = MapToItem(itemDetails);
        var count = bagContentData.Count;
        var totalPrice = count * price;

        var bagContent = new BagContent(bagContentData.Id, item, availableCount, price, count, totalPrice);
        return bagContent;
    }

    private static PickupPoint MapToPickupPoint(PickupPointDetails pickupPointDetails)
    {
        var pickupPoint = new PickupPoint(pickupPointDetails.Description, pickupPointDetails.Address);
        return pickupPoint;
    }

    private static Item MapToItem(ItemDetails itemDetails)
    {
        var item = new Item(itemDetails.Name, itemDetails.Description, itemDetails.PhotoLink);
        return item;
    }
}