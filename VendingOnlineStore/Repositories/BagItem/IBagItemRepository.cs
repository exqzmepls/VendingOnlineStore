namespace VendingOnlineStore.Repositories.BagItem;

public interface IBagItemRepository
{
    public string? Add(string externalId, string vendingMachineId);

    public int? GetCount(string id);

    public int? UpdateCount(string id, int shift);

    public bool Delete(string id);
}
