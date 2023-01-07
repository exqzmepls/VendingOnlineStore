using Core.Services.Vending;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models;

namespace VendingOnlineStore.Controllers;
public class ItemController : Controller
{
    private readonly IVendingService _vendingService;

    public ItemController(IVendingService vendingService)
    {
        _vendingService = vendingService;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var items = await _vendingService.GetItemsAsync();
        var itemsModel = items.Select(i =>
        {
            var model = new ItemViewModel(i.Id, i.Name, i.Description, i.PhotoLink, i.PriceTag);
            return model;
        });
        return View(itemsModel);
    }

    [HttpGet]
    public async Task<IActionResult> MachinesAsync(string itemId)
    {
        var machines = await _vendingService.GetItemMachinesAsync(itemId);
        var machinesModel = machines.Select(m =>
        {
            var model = new ItemMachineViewModel(m.Id, m.Address, m.Distance, m.ItemPrice);
            return model;
        });
        return View(machinesModel);
    }
}
