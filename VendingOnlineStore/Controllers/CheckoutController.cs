using Core.Extensions;
using Core.Services.Checkout;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models.Checkout;

namespace VendingOnlineStore.Controllers;

public class CheckoutController : Controller
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpPost]
    public async Task<IActionResult> DetailsAsync([FromForm] CheckoutViewModel checkoutViewModel)
    {
        var checkoutDetails = await _checkoutService.GetCheckoutOrDefaultAsync(checkoutViewModel.BagSectionId);
        if (checkoutDetails == default)
        {
            return NotFound();
        }

        var model = Map(checkoutDetails);
        return View(model);
    }

    private static CheckoutDetailsViewModel Map(CheckoutDetails checkoutDetails)
    {
        var pickupPointInfoViewModel = MapToPickupPointViewModel(checkoutDetails.PickupPoint);
        var contentsPreviewsViewModel = checkoutDetails.Contents
            .Select(MapToContentDetailsViewModel)
            .ToReadOnlyCollection();
        var purchasePreviewViewModel = new CheckoutDetailsViewModel(
            checkoutDetails.BagSectionId,
            pickupPointInfoViewModel,
            contentsPreviewsViewModel,
            checkoutDetails.TotalPrice
        );
        return purchasePreviewViewModel;
    }

    private static PickupPointViewModel MapToPickupPointViewModel(PickupPoint pickupPoint)
    {
        var pickupPointViewModel = new PickupPointViewModel(pickupPoint.Address, pickupPoint.Description);
        return pickupPointViewModel;
    }

    private static ContentDetailsViewModel MapToContentDetailsViewModel(ContentDetails contentDetails)
    {
        var itemViewModel = MapToItemViewModel(contentDetails.Item);
        var contentDetailsViewModel = new ContentDetailsViewModel(
            contentDetails.BagContentId,
            itemViewModel,
            contentDetails.WantedCount,
            contentDetails.SuggestedCount,
            contentDetails.TotalPrice
        );
        return contentDetailsViewModel;
    }

    private static ItemViewModel MapToItemViewModel(Item itemInfo)
    {
        var itemInfoViewModel = new ItemViewModel(
            itemInfo.Name,
            itemInfo.PhotoLink,
            itemInfo.AvailableCount,
            itemInfo.Price
        );
        return itemInfoViewModel;
    }
}