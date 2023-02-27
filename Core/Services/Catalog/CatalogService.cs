using Core.Clients.Catalog;
using Core.Extensions;
using Core.Identity;
using Core.Repositories.BagContent;
using Core.Repositories.BagSection;
using Core.Services.Catalog.Exceptions;

namespace Core.Services.Catalog;

public class CatalogService : ICatalogService
{
    private readonly ICatalogClient _catalogClient;
    private readonly IBagSectionRepository _bagSectionRepository;
    private readonly IBagContentRepository _bagContentRepository;
    private readonly IUserIdentityProvider _userIdentityProvider;

    public CatalogService(
        ICatalogClient catalogClient,
        IBagSectionRepository bagSectionRepository,
        IBagContentRepository bagContentRepository,
        IUserIdentityProvider userIdentityProvider
    )
    {
        _catalogClient = catalogClient;
        _bagSectionRepository = bagSectionRepository;
        _bagContentRepository = bagContentRepository;
        _userIdentityProvider = userIdentityProvider;
    }

    public Location GetDefaultLocation()
    {
        return new Location(58.009535, 56.224404, 250);
    }

    public IReadOnlyCollection<OptionDetails> GetOptions(Location location)
    {
        var userId = _userIdentityProvider.GetUserIdentifier();

        var locationData = new LocationData(location.Latitude, location.Longitude, location.Radius);
        var options = _catalogClient.GetOptions(locationData);

        var bagSections = _bagSectionRepository.GetAll()
            .Where(s => s.UserId == userId)
            .ToReadOnlyCollection();

        var result = GetOptionsDetails(options, bagSections).ToReadOnlyCollection();
        return result;
    }

    public async Task<BagContent> AddToBagAsync(NewBagContent newBagContent)
    {
        var pickupPointId = newBagContent.PickupPointId;
        var itemId = newBagContent.ItemId;
        var bagContentId = await AddNewBagContentAsync(pickupPointId, itemId);

        var bagEntrance = await GetBagEntranceAsync(bagContentId);

        var bagContent = new BagContent(itemId, pickupPointId, bagEntrance);
        return bagContent;
    }

    public async Task<BagContent> IncreaseContentCountAsync(Guid bagContentId)
    {
        var bagContentDetailsData = await GetBagContentDetailsDataAsync(bagContentId);

        var updatedCount = bagContentDetailsData.Count + 1;
        await UpdateBagContentCountAsync(bagContentDetailsData.Id, updatedCount);

        var bagEntrance = await GetBagEntranceAsync(bagContentId);

        var itemId = bagContentDetailsData.ItemId;
        var pickupPointId = bagContentDetailsData.Section.PickupPointId;
        var bagContent = new BagContent(itemId, pickupPointId, bagEntrance);
        return bagContent;
    }

    public async Task<BagContent> DecreaseContentCountAsync(Guid bagContentId)
    {
        var bagContentDetailsData = await GetBagContentDetailsDataAsync(bagContentId);

        var updatedCount = bagContentDetailsData.Count - 1;
        if (updatedCount == 0)
        {
            await RemoveContentAsync(bagContentDetailsData.Id);
        }
        else
        {
            await UpdateBagContentCountAsync(bagContentDetailsData.Id, updatedCount);
        }

        var bagEntrance = await GetBagEntranceAsync(bagContentId);

        var itemId = bagContentDetailsData.ItemId;
        var pickupPointId = bagContentDetailsData.Section.PickupPointId;
        var bagContent = new BagContent(itemId, pickupPointId, bagEntrance);
        return bagContent;
    }

    private static IEnumerable<OptionDetails> GetOptionsDetails(IEnumerable<Option> options,
        IReadOnlyCollection<BagSectionDetailsData> bagSections)
    {
        var result = options.Select(option =>
        {
            var optionContents = GetOptionContents(option.Contents, option.Item.Id, bagSections).ToReadOnlyCollection();
            var itemDetails = MapToItemDetails(option.Item);
            var optionDetails = new OptionDetails(itemDetails, optionContents);
            return optionDetails;
        });
        return result;
    }

    private static IEnumerable<ContentDetails> GetOptionContents(IEnumerable<Content> contents, string optionItemId,
        IReadOnlyCollection<BagSectionDetailsData> bagSections)
    {
        var result = contents.Select(content =>
        {
            var bagContent = GetBagContent(bagSections, content.PickupPoint.Id, optionItemId);
            var pickupPointDetails = MapToPickupPointDetails(content.PickupPoint);
            var optionContent = new ContentDetails(pickupPointDetails, content.Price, bagContent);
            return optionContent;
        });
        return result;
    }

    private static BagContent GetBagContent(IEnumerable<BagSectionDetailsData> bagSections, string pickupPointId,
        string itemId)
    {
        var section = bagSections.SingleOrDefault(bagSection => bagSection.PickupPointId == pickupPointId);
        if (section == default)
            return new BagContent(itemId, pickupPointId, default);

        var bagEntrance = GetBagEntrance(section.Contents, itemId);
        var bagContent = new BagContent(itemId, pickupPointId, bagEntrance);
        return bagContent;
    }

    private static BagEntrance? GetBagEntrance(IEnumerable<BagContentBriefData> bagSectionContents, string itemId)
    {
        var content = bagSectionContents.SingleOrDefault(bagContent => bagContent.ItemId == itemId);
        if (content == default)
            return default;

        var bagEntrance = new BagEntrance(content.Id, content.Count);
        return bagEntrance;
    }

    private static PickupPointDetails MapToPickupPointDetails(PickupPoint pickupPoint)
    {
        var pickupPointDetails = new PickupPointDetails(
            pickupPoint.Address,
            pickupPoint.Description
        );
        return pickupPointDetails;
    }

    private static ItemDetails MapToItemDetails(Item item)
    {
        var itemDetails = new ItemDetails(
            item.Name,
            item.Description,
            item.PhotoLink
        );
        return itemDetails;
    }

    private async Task<Guid> AddNewBagContentAsync(string pickupPointId, string itemId)
    {
        var userId = _userIdentityProvider.GetUserIdentifier();

        var section = _bagSectionRepository.GetAll()
            .Where(s => s.UserId == userId)
            .SingleOrDefault(s => s.PickupPointId == pickupPointId);

        const int initialContentCount = 1;
        if (section == default)
        {
            var newContents = new NewBagSectionContentData[]
            {
                new(itemId, initialContentCount)
            };
            var newBagSection = new NewBagSectionData(userId, pickupPointId, newContents);
            var newBagSectionBriefData = await _bagSectionRepository.CreateAsync(newBagSection);
            var singleBagContentId = newBagSectionBriefData.ContentsIds.Single();
            return singleBagContentId;
        }

        var newBagContentData = new NewBagContentData(section.Id, itemId, initialContentCount);
        var bagContentId = await _bagContentRepository.CreateAsync(newBagContentData);
        return bagContentId;
    }

    private async Task<BagEntrance?> GetBagEntranceAsync(Guid bagContentId)
    {
        var userId = _userIdentityProvider.GetUserIdentifier();

        var bagContentData = await _bagContentRepository.GetOrDefaultAsync(bagContentId);
        if (bagContentData == default || bagContentData.Section.UserId != userId)
            return default;

        var bagEntrance = new BagEntrance(bagContentData.Id, bagContentData.Count);
        return bagEntrance;
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

    private async Task RemoveContentAsync(Guid contentId)
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
}