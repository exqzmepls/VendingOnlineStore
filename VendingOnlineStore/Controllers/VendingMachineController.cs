using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models;
using VendingOnlineStore.Services.Vending;

namespace VendingOnlineStore.Controllers;

public class VendingMachineController : Controller
{
    private readonly IVendingService _vendingService;

    public VendingMachineController(IVendingService vendingService)
    {
        _vendingService = vendingService;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var machines = await _vendingService.GetMachinesAsync();
        var machinesModels = machines.Select(m =>
        {
            var model = new VendingMachineViewModel(m.Id, m.Description, m.Address, m.Distance);
            return model;
        });

        return View(machinesModels);
    }

    public async Task<IActionResult> SlotsAsync(string machineId)
    {
        var slots = await _vendingService.GetMachineSlotsAsync(machineId);
        var slotsModels = slots.Select(s =>
        {
            var model = new SlotViewModel(s.Id, s.ItemName, s.ItemDescription, s.ItemPhotoLink, s.Price, s.Count);
            return model;
        });

        return View(slotsModels);
    }
}
