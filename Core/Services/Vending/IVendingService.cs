using Core.Services.Vending.Dtos;

namespace Core.Services.Vending;

public interface IVendingService
{
    public Task<IEnumerable<VendingMachine>> GetMachinesAsync();

    public Task<IEnumerable<MachineSlot>> GetMachineSlotsAsync(string machineId);

    public Task<IEnumerable<Item>> GetItemsAsync();

    public Task<IEnumerable<ItemMachine>> GetItemMachinesAsync(string itemId);
}
