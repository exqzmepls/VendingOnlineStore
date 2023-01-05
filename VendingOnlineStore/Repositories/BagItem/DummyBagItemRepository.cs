using VendingOnlineStore.Data;

namespace VendingOnlineStore.Repositories.BagItem;

public class DummyBagItemRepository : IBagItemRepository
{
    public string? Add(string externalId, string vendingMachineId)
    {
        var machine = DummyBagStorage.Content.SingleOrDefault(x => x.Id == vendingMachineId);
        if (machine == default)
            return default;

        machine
    }

    public bool Delete(string id)
    {
        foreach (var machine in DummyBagStorage.Content)
        {
            var item = machine.Items.SingleOrDefault(x => x.Id == id);
            if (item == default)
                continue;

            var result = machine.Items
        }
    }

    public int UpdateCount(string id, int shift)
    {
        throw new NotImplementedException();
    }
}
