using VendingOnlineStore.Services.Bag.Dto;

namespace VendingOnlineStore.Services.Bag;

public interface IBagService
{
    public Task AddItemAsync(string itemId);

    public Task RemoveItemAsync(string itemId);

    public Task IncreaseItemCountAsync(string itemId);

    public Task DecreaseItemCountAsync(string itemId);

    public Task<IEnumerable<BagMachine>> GetContentAsync();
}
