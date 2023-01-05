namespace VendingOnlineStore.Repositories.Bag;

public interface IBagRepository
{
    public IEnumerable<BagItem> GetlAll();

    public int UpdateCount(string id, int shift);

    public string? Add(string itemId, string vendingMachineId);

    public bool Delete(string id);
}
