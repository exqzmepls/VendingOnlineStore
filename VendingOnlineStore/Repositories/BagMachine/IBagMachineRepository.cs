using VendingOnlineStore.Repositories.BagMachine.Dtos;

namespace VendingOnlineStore.Repositories.BagMachine;

public interface IBagMachineRepository
{
    public IQueryable<BagMachineDto> GetAll();

    public Task<BagMachineDto?> GetOrDefaultAsync(Guid id);

    public Task<bool> DeleteAsync(Guid id);
}
