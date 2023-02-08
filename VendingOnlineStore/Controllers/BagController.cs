using Core.Clients.Payment;
using Core.Services.Bag;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models.Bag;

namespace VendingOnlineStore.Controllers;

public class BagController : Controller
{
    private readonly IBagService _bagService;
    private readonly IPaymentClient _paymentClient;

    public BagController(IBagService bagService, IPaymentClient paymentClient)
    {
        _bagService = bagService;
        _paymentClient = paymentClient;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var model = await GetBagSectionsViewModelsAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> IncreaseItemCountAsync(Guid itemId)
    {
        try
        {
            await _bagService.IncreaseContentCountAsync(itemId);
        }
        catch
        {
            return BadRequest();
        }

        var partialView = await GetBagSectionsPartialViewAsync();
        return partialView;
    }

    [HttpPost]
    public async Task<IActionResult> DecreaseItemCountAsync(Guid itemId)
    {
        try
        {
            await _bagService.DecreaseContentCountAsync(itemId);
        }
        catch
        {
            return BadRequest();
        }

        var partialView = await GetBagSectionsPartialViewAsync();
        return partialView;
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItemAsync(Guid itemId)
    {
        try
        {
            await _bagService.RemoveContentAsync(itemId);
        }
        catch
        {
            return BadRequest();
        }

        var partialView = await GetBagSectionsPartialViewAsync();
        return partialView;
    }

    [Obsolete("Relocation needed")]
    public async Task<IActionResult> Buy(string id)
    {
        var paymentDetails = await _paymentClient.CreatePaymentAsync();
        return Redirect(paymentDetails.Link);
    }

    [Obsolete("Relocation needed")]
    public async Task<IActionResult> BuyItem(decimal price)
    {
        var paymentDetails = await _paymentClient.CreatePaymentAsync(price);
        return Redirect(paymentDetails.Link);
    }

    private static BagSectionViewModel MapToBagSectionViewModel(BagSection bagSection)
    {
        var pickupPoint = bagSection.PickupPoint;
        var items = bagSection.Contents.Select(MapToBagContentViewModel);
        var bagMachineViewModel = new BagSectionViewModel(bagSection.Id, pickupPoint.Description, pickupPoint.Address,
            items, bagSection.TotalPrice);
        return bagMachineViewModel;
    }

    private static BagContentViewModel MapToBagContentViewModel(BagContent bagContent)
    {
        var item = bagContent.Item;
        var bagItemViewModel = new BagContentViewModel(bagContent.Id, item.Name, item.Description, item.PhotoLink,
            bagContent.AvailableCount, bagContent.Price, bagContent.Count, bagContent.TotalPrice);
        return bagItemViewModel;
    }

    private async Task<PartialViewResult> GetBagSectionsPartialViewAsync()
    {
        var model = await GetBagSectionsViewModelsAsync();
        return PartialView("_BagSections", model);
    }

    private async Task<IEnumerable<BagSectionViewModel>> GetBagSectionsViewModelsAsync()
    {
        var content = await _bagService.GetSectionsAsync();
        var model = content.Select(MapToBagSectionViewModel);
        return model;
    }
}