using Core.Clients.PickupPoint;
using Core.Extensions;
using Core.Identity;
using Core.Repositories.BagSection;
using Core.Services.Checkout;
using Item = Core.Services.Checkout.Item;
using PickupPoint = Core.Services.Checkout.PickupPoint;

namespace Application.Services;

internal class CheckoutService : ICheckoutService
{
    private readonly IBagSectionRepository _bagSectionRepository;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IPickupPointClient _pickupPointClient;

    public CheckoutService(
        IBagSectionRepository bagSectionRepository,
        IUserIdentityProvider userIdentityProvider,
        IPickupPointClient pickupPointClient
    )
    {
        _bagSectionRepository = bagSectionRepository;
        _userIdentityProvider = userIdentityProvider;
        _pickupPointClient = pickupPointClient;
    }

    public async Task<CheckoutDetails?> GetCheckoutOrDefaultAsync(Guid bagSectionId)
    {
        var userId = _userIdentityProvider.GetUserIdentifier();

        // get section contents
        var bagSectionData = await _bagSectionRepository.GetByIdOrDefaultAsync(bagSectionId);
        if (bagSectionData == default || bagSectionData.UserId != userId)
        {
            return default;
        }

        // get price and available count
        var itemsIds = bagSectionData.Contents.Select(bagContentData => bagContentData.ItemId);
        var specification = new PickupPointContentsSpecification(bagSectionData.PickupPointId, itemsIds);
        var pickupPointPresentation = await _pickupPointClient.GetPresentationAsync(specification);

        // staff to validate
        var pickupPoint = pickupPointPresentation.PickupPoint;
        var pickupPointInfo = new PickupPoint(pickupPoint.Address, pickupPoint.Description);

        var contents = bagSectionData.Contents.Select(bagContentData =>
            {
                var content = GetContentFromPickupPointPresentation(bagContentData, pickupPointPresentation);
                return content;
            })
            .ToReadOnlyCollection();

        var totalPrice = contents.Sum(c => c.TotalPrice);

        // create result obj
        var checkout = new CheckoutDetails(bagSectionData.Id, pickupPointInfo, contents, totalPrice);
        return checkout;
    }

    private static ContentDetails GetContentFromPickupPointPresentation(BagContentBriefData bagContentData,
        PickupPointPresentation presentation)
    {
        var itemId = bagContentData.ItemId;
        var (itemDetails, availableCount, itemPrice) = presentation.Contents.Single(c => c.Item.Id == itemId);

        var item = new Item(itemDetails.Name, itemDetails.PhotoLink, availableCount, itemPrice);
        var wantedCount = bagContentData.Count;
        var suggestedCount = wantedCount > availableCount ? availableCount : wantedCount;
        var totalPrice = suggestedCount * itemPrice;

        var content = new ContentDetails(bagContentData.Id, item, wantedCount, suggestedCount, totalPrice);
        return content;
    }
}