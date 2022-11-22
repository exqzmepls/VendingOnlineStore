using VendingOnlineStore.Clients.Vending.Dtos;

namespace VendingOnlineStore.Clients.Vending;

public interface IVendingClient
{
    [Obsolete("Not implemented")]
    public Task<IEnumerable<VendingMachine>> GetMachinesAsync();

    [Obsolete("Not implemented")]
    public Task<IEnumerable<Slot>> GetMachineSlotsAsync(string machineId);

    public Task<IEnumerable<Item>> GetItemsAsync();

    public Task<IEnumerable<ItemMachine>> GetItemMachinesAsync(string itemId);
}
