using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Clients.Payment;
using VendingOnlineStore.Models.Bag;
using VendingOnlineStore.Services.Bag;
using VendingOnlineStore.Services.Bag.Dtos;

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
        var model = await GetContentModelAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> IncreaseItemCountAsync(Guid itemId)
    {
        var isSuccess = await _bagService.IncreaseItemCountAsync(itemId);
        if (!isSuccess)
        {
            return BadRequest();
        }

        var partialView = await GetContentPartialViewAsync();
        return partialView;
    }

    [HttpPost]
    public async Task<IActionResult> DecreaseItemCountAsync(Guid itemId)
    {
        var isSuccess = await _bagService.DecreaseItemCountAsync(itemId);
        if (!isSuccess)
        {
            return BadRequest();
        }

        var partialView = await GetContentPartialViewAsync();
        return partialView;
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItemAsync(Guid itemId)
    {
        var isSuccess = await _bagService.RemoveItemAsync(itemId);
        if (!isSuccess)
        {
            return BadRequest();
        }

        var partialView = await GetContentPartialViewAsync();
        return partialView;
    }

    public async Task<IActionResult> Buy(string id)
    {
        var url = await _paymentClient.CreatePayment();
        return Redirect(url);
    }

    public async Task<IActionResult> BuyItem(decimal price)
    {
        var url = await _paymentClient.CreatePayment(price);
        return Redirect(url);
    }

    private static BagMachineViewModel Map(BagMachine bagMachine)
    {
        var machineInfo = bagMachine.MachineInfo;
        var items = bagMachine.MachineItems.Select(Map);
        var bagMachineViewModel = new BagMachineViewModel(bagMachine.Id, machineInfo.Description, machineInfo.Address, items, bagMachine.TotalPrice);
        return bagMachineViewModel;
    }

    private static BagItemViewModel Map(BagItem bagItem)
    {
        var itemInfo = bagItem.ItemInfo;
        var machineItemInfo = bagItem.MachineItemInfo;
        var bagItemViewModel = new BagItemViewModel(bagItem.Id, itemInfo.Name, itemInfo.Description, itemInfo.PhotoLink, machineItemInfo.AvailableCount, machineItemInfo.Price, bagItem.Count, bagItem.TotalPrice);
        return bagItemViewModel;
    }

    private async Task<PartialViewResult> GetContentPartialViewAsync()
    {
        var model = await GetContentModelAsync();
        return PartialView("_BagContent", model);
    }

    private async Task<IEnumerable<BagMachineViewModel>> GetContentModelAsync()
    {
        var content = await _bagService.GetContentAsync();
        var model = content.Select(Map);
        return model;
    }
}
