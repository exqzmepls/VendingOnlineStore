using VendingOnlineStore.Clients.Vending;
using VendingOnlineStore.Repositories.BagItem;
using VendingOnlineStore.Repositories.BagMachine;
using VendingOnlineStore.Services.Bag.Dto;

namespace VendingOnlineStore.Services.Bag;

public class DummyBagService : IBagService
{
    private readonly IBagMachineRepository _bagMachineRepository;
    private readonly IBagItemRepository _bagItemRepository;
    private readonly IVendingClient _vendingClient;

    public DummyBagService(IBagMachineRepository bagMachineRepository, IBagItemRepository bagItemRepository, IVendingClient vendingClient)
    {
        _bagMachineRepository = bagMachineRepository;
        _bagItemRepository = bagItemRepository;
        _vendingClient = vendingClient;
    }

    public async Task AddItemAsync(string itemId)
    {
        await Task.Delay(35);
        _bagItemRepository.Add(itemId);
    }

    public async Task DecreaseItemCountAsync(string itemId)
    {
        await Task.Delay(35);
        var count = _bagItemRepository.GetCount(itemId);

        if (count == 1)
        {
            await RemoveItemAsync(itemId);
            return;
        }

        var currentCount = _bagItemRepository.UpdateCount(itemId, -1);
    }

    public async Task<IEnumerable<BagMachine>> GetContentAsync()
    {
        await Task.Delay(35);
        var content = _bagMachineRepository.GetAll();
        var result = content.Select(m =>
        {
            var machineInfo = _vendingClient.
        })
        return content;
    }

    public async Task IncreaseItemCountAsync(string itemId)
    {
        await Task.Delay(35);
        var count = _items[itemId];
        _items[itemId] = count + 1;
    }

    public async Task RemoveItemAsync(string itemId)
    {
        await Task.Delay(35);


        _bagItemRepository.Delete(itemId);
    }
}
