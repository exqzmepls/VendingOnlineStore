using VendingOnlineStore.Data;
using VendingOnlineStore.Repositories.BagMachine.Dtos;

namespace VendingOnlineStore.Repositories.BagMachine;

public class DummyBagMachineRepository : IBagMachineRepository
{
    public string? Add(string externalId)
    {
        var id = Guid.NewGuid().ToString();
        var newMachine = new BagMachineDto(id, externalId, Enumerable.Empty<BagItemDto>());
        DummyBagStorage.Content.Add(newMachine);
        return id;
    }

    public bool Delete(string id)
    {
        var machine = DummyBagStorage.Content.Single(x => x.Id == id);
        var result = DummyBagStorage.Content.Remove(machine);
        return result;
    }

    public IEnumerable<BagMachineDto> GetAll()
    {
        return DummyBagStorage.Content.AsEnumerable();
    }
}
