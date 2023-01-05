namespace VendingOnlineStore.Repositories.Bag;

public class BagItem
{
    public BagItem(string id, string itemId, string vendingMachineId, int count)
    {
        Id = id;
        ItemId = itemId;
        VendingMachineId = vendingMachineId;
        Count = count;
    }

    public string Id { get; }
    public string ItemId { get; }
    public string VendingMachineId { get; }
    public int Count { get; }
}
