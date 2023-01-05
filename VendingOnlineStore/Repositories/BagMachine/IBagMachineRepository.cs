using VendingOnlineStore.Repositories.BagMachine.Dtos;

namespace VendingOnlineStore.Repositories.BagMachine;

public interface IBagMachineRepository
{
    public IEnumerable<BagMachineDto> GetAll();

    public string? Add(string externalId);

    public bool Delete(string id);
}
