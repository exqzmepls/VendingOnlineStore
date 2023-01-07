using Core.Services.Bag.Dtos;

namespace Core.Services.Bag;

public interface IBagService
{
    public Task<IEnumerable<BagMachine>> GetContentAsync();

    public Task<bool> RemoveItemAsync(Guid itemId);

    public Task<bool> IncreaseItemCountAsync(Guid itemId);

    public Task<bool> DecreaseItemCountAsync(Guid itemId);
}
