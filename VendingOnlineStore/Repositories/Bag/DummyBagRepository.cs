namespace VendingOnlineStore.Repositories.Bag;

public class DummyBagRepository : IBagRepository
{
    private readonly IList<BagItem> _bagItems = new List<BagItem>
    {
        new BagItem("1", "i1", "m1", 1),
        new BagItem("2", "i2", "m1", 2),
        new BagItem("3", "i3", "m2", 1)
    };

    public string? Add(string itemId, string vendingMachineId)
    {
        var isExist = _bagItems.Any(b => b.ItemId == itemId && b.VendingMachineId == vendingMachineId);
        if (isExist)
            return default;

        var newBagItem = new BagItem("id", itemId, vendingMachineId, 1);
        _bagItems.Add(newBagItem);
        return newBagItem.Id;
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BagItem> GetlAll()
    {
        return _bagItems.AsEnumerable();
    }

    public int UpdateCount(string id, int shift)
    {
        var item = 
    }
}
