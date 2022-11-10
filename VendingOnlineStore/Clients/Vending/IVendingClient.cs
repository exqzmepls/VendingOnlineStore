using VendingOnlineStore.Clients.Vending.Dtos;

namespace VendingOnlineStore.Clients.Vending;

public interface IVendingClient
{
    public Task<IEnumerable<VendingMachine>> GetMachinesAsync();

    public Task<IEnumerable<Slot>> GetMachineSlotsAsync(string machineId);
}
