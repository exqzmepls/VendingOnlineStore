using Core.Services.Vending.Dtos;

namespace Core.Services.Vending;

public class VendingService : IVendingService
{
    private readonly IVendingClient _vendingClient;
    private readonly IGeoClient _geoClient;

    public VendingService(IVendingClient vendingClient, IGeoClient geoClient)
    {
        _vendingClient = vendingClient;
        _geoClient = geoClient;
    }

    public async Task<IEnumerable<ItemMachine>> GetItemMachinesAsync(string itemId)
    {
        var receivedMachines = await _vendingClient.GetItemMachinesAsync(itemId);
        var machines = receivedMachines.Select(m =>
        {
            var distance = _geoClient.GetDistance(m.Latitude, m.Longitude);
            var machine = new ItemMachine(m.Id, m.Address, $"{Math.Round(distance ?? 0)} m", m.ItemPrice);
            return machine;
        });
        return machines;
    }

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        var receivedItems = await _vendingClient.GetItemsAsync();
        var items = receivedItems.Select(i =>
        {
            var priceTag = $"at {i.MinPrice}";
            var item = new Item(i.Id, i.Name, i.Description, i.PhotoLink, priceTag);
            return item;
        });
        return items;
    }

    public async Task<IEnumerable<VendingMachine>> GetMachinesAsync()
    {
        var receivedMachines = await _vendingClient.GetMachinesAsync();
        var machines = receivedMachines.Select(m =>
        {
            var distance = _geoClient.GetDistance(m.Latitude, m.Longitude);
            var machine = new VendingMachine(m.Id, m.Description, m.Address, $"{Math.Round(distance ?? 0)} m");
            return machine;
        });
        return machines;
    }

    public async Task<IEnumerable<MachineSlot>> GetMachineSlotsAsync(string machineId)
    {
        var receivedSlots = await _vendingClient.GetMachineSlotsAsync(machineId);
        var slots = receivedSlots.Select(s =>
        {
            var item = s.Item;
            var slot = new MachineSlot(s.Id, item.Name, item.Description, item.PhotoLink, s.Price, s.Count);
            return slot;
        });
        return slots;
    }


}
