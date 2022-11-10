using VendingOnlineStore.Services.Vending.Dtos;

namespace VendingOnlineStore.Services.Vending;

public interface IVendingService
{
    public Task<IEnumerable<VendingMachine>> GetMachinesAsync();

    public Task<IEnumerable<MachineSlot>> GetMachineSlotsAsync(string machineId);
}
