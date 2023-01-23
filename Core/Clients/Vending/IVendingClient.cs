using Core.Clients.Vending.Dtos;

namespace Core.Clients.Vending;

public interface IVendingClient
{
    [Obsolete("Not implemented")]
    public Task<IEnumerable<VendingMachine>> GetMachinesAsync();

    [Obsolete("Not implemented")]
    public Task<IEnumerable<Slot>> GetMachineSlotsAsync(string machineId);

    public Task<IEnumerable<Item>> GetItemsAsync();

    public Task<Item> GetItemAsync(string itemId);

    public Task<IEnumerable<ItemMachine>> GetItemMachinesAsync(string itemId);

    public Task<IEnumerable<MachineItemsInfo>> GetMachinesItemsInfoAsync(IEnumerable<MachineItemsQuery> machineItems);

    public Task<string> CreateBookingAsync(string pickupPointId, IEnumerable<ItemInfo> content);

    public Task<ObtainingDetails> ConfirmBookingAsync(string bookingId);

    public Task DropBookingAsync(string bookingId);
}