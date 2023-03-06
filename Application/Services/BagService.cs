using Core.Clients.PickupPoint;
using Core.Extensions;
using Core.Identity;
using Core.Repositories.BagContent;
using Core.Repositories.BagSection;
using Core.Services.Bag;
using Core.Services.Bag.Exceptions;
using Item = Core.Services.Bag.Item;
using PickupPointData = Core.Clients.PickupPoint.PickupPoint;
using ItemData = Core.Clients.PickupPoint.Item;
using PickupPoint = Core.Services.Bag.PickupPoint;

namespace Application.Services;

internal class BagService : IBagService
{
    private readonly IBagSectionRepository _bagSectionRepository;
    private readonly IBagContentRepository _bagContentRepository;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IPickupPointClient _pickupPointClient;

    public BagService(
        IBagSectionRepository bagSectionRepository,
        IBagContentRepository bagContentRepository,
        IUserIdentityProvider userIdentityProvider,
        IPickupPointClient pickupPointClient
    )
    {
        _bagSectionRepository = bagSectionRepository;
        _bagContentRepository = bagContentRepository;
        _userIdentityProvider = userIdentityProvider;
        _pickupPointClient = pickupPointClient;
    }

    public async Task DecreaseContentCountAsync(Guid contentId)
    {
        var bagContentDetailsData = await GetBagContentDetailsDataAsync(contentId);

        var updatedCount = bagContentDetailsData.Count - 1;
        if (updatedCount == 0)
        {
            await RemoveContentAsync(bagContentDetailsData.Id);
            return;
        }

        await UpdateBagContentCountAsync(bagContentDetailsData.Id, updatedCount);
    }

    public async Task<IReadOnlyCollection<BagSection>> GetSectionsAsync()
    {
        var userId = _userIdentityProvider.GetUserIdentifier();

        // get sections data
        var bagSectionsData = _bagSectionRepository.GetAll()
            .Where(s => s.UserId == userId)
            .ToReadOnlyCollection();

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
        var pickupPointsPresentations = await _pickupPointClient.GetPresentationsAsync(specifications);

        var bagSections = bagSectionsData.Select(bagSectionData =>
            {
                var bagSection = GetBagSectionFromPickupPointsPresentations(bagSectionData, pickupPointsPresentations);
                return bagSection;
            })
            .OrderBy(m => m.Id)
            .ToReadOnlyCollection();
        return bagSections;
    }

    public async Task IncreaseContentCountAsync(Guid contentId)
    {
        var bagContentDetailsData = await GetBagContentDetailsDataAsync(contentId);

        var updatedCount = bagContentDetailsData.Count + 1;
        await UpdateBagContentCountAsync(bagContentDetailsData.Id, updatedCount);
    }

    public async Task RemoveContentAsync(Guid contentId)
    {
        var bagContentDetailsData = await GetBagContentDetailsDataAsync(contentId);

        var contentSectionId = bagContentDetailsData.Section.Id;
        var bagSectionDetails = await _bagSectionRepository.GetByIdOrDefaultAsync(contentSectionId);
        if (bagSectionDetails == default)
        {
            throw new SectionNotFoundException("Section does not exist");
        }

        var sectionContentsCount = bagSectionDetails.Contents.Count;
        if (sectionContentsCount == 1)
        {
            try
            {
                await _bagSectionRepository.DeleteAsync(contentSectionId);
                return;
            }
            catch (Exception exception)
            {
                throw new SectionNotDeletedException("Section delete fell", exception);
            }
        }

        try
        {
            await _bagContentRepository.DeleteAsync(bagContentDetailsData.Id);
        }
        catch (Exception exception)
        {
            throw new ContentNotDeletedException("Content delete fell", exception);
        }
    }

    private static BagSection GetBagSectionFromPickupPointsPresentations(BagSectionDetailsData bagSectionData,
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

    private static BagContent GetBagContentFromPickupPointContents(BagContentBriefData bagContentData,
        IEnumerable<PickupPointContent> pickupPointContents)
    {
        var itemId = bagContentData.ItemId;
        var (itemDetails, availableCount, price) = pickupPointContents.Single(c => c.Item.Id == itemId);

        var item = MapToItem(itemDetails);
        var count = bagContentData.Count;
        var totalPrice = count * price;

        var bagContent = new BagContent(bagContentData.Id, item, availableCount, price, count, totalPrice);
        return bagContent;
    }

    private static PickupPoint MapToPickupPoint(PickupPointData pickupPointData)
    {
        var pickupPoint = new PickupPoint(pickupPointData.Description, pickupPointData.Address);
        return pickupPoint;
    }

    private static Item MapToItem(ItemData itemData)
    {
        var item = new Item(itemData.Name, itemData.Description, itemData.PhotoLink);
        return item;
    }

    private async Task<BagContentDetailsData> GetBagContentDetailsDataAsync(Guid id)
    {
        var userId = _userIdentityProvider.GetUserIdentifier();

        var bagContentDetailsData = await _bagContentRepository.GetOrDefaultAsync(id);
        if (bagContentDetailsData == default || bagContentDetailsData.Section.UserId != userId)
        {
            throw new ContentNotFoundException("Content does not exist.");
        }

        return bagContentDetailsData;
    }

    private async Task UpdateBagContentCountAsync(Guid contentId, int updatedCount)
    {
        var bagContentUpdate = new BagContentUpdate(updatedCount);
        try
        {
            await _bagContentRepository.UpdateAsync(contentId, bagContentUpdate);
        }
        catch (Exception exception)
        {
            throw new ContentNotUpdatedException("Content update fell", exception);
        }
    }
}