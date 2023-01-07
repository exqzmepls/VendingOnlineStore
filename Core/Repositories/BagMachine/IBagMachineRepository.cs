using Core.Repositories.BagMachine.Dtos;

namespace Core.Repositories.BagMachine;

public interface IBagMachineRepository
{
    public IQueryable<BagMachineDto> GetAll();

    public Task<BagMachineDto?> GetOrDefaultAsync(Guid id);

    public Task<bool> DeleteAsync(Guid id);
}
