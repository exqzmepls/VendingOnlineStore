using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Clients.Payment;
using VendingOnlineStore.Models;
using VendingOnlineStore.Services.Bag;

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
        var items = await _bagService.GetContentAsync(); 
        var itemsModel = items.Select(i =>
        {
            var model = new BagItemViewModel(i.Id, i.Name, i.PhotoLink, i.Count);
            return model;
        });
        return View(itemsModel);
    }

    [HttpGet]
    public async Task<IActionResult> RemoveItemAsync(string itemId)
    {
        await _bagService.RemoveItemAsync(itemId);
        return RedirectToAction("Index");
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
}
