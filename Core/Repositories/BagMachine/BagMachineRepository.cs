using Core.Repositories.BagMachine.Dtos;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using BagItemEntity = Infrastructure.Models.BagItem;
using BagMachineEntity = Infrastructure.Models.BagMachine;

namespace Core.Repositories.BagMachine;

public class BagMachineRepository : IBagMachineRepository
{
    private readonly AppDbContext _appDbContext;

    public BagMachineRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bagMachineEntity = await _appDbContext.BagMachines.FindAsync(id);
        if (bagMachineEntity == default)
        {
            return false;
        }

        _appDbContext.BagMachines.Remove(bagMachineEntity);
        try
        {
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public IQueryable<BagMachineDto> GetAll()
    {
        var bagMachines = _appDbContext.BagMachines.Include(m => m.BagItems);
        var result = bagMachines.Select(m => Map(m));
        return result;
    }

    public async Task<BagMachineDto?> GetOrDefaultAsync(Guid id)
    {
        var bagMachineEntity = await _appDbContext.BagMachines.FindAsync(id);
        if (bagMachineEntity == default)
        {
            return default;
        }

        _appDbContext.Entry(bagMachineEntity).Collection(m => m.BagItems!).Load();
        var bagMachine = Map(bagMachineEntity);
        return bagMachine;
    }

    private static BagMachineDto Map(BagMachineEntity bagMachineEntity)
    {
        var bagMachine = new BagMachineDto(bagMachineEntity.Id, bagMachineEntity.ExternalId, bagMachineEntity.BagItems!.Select(Map));
        return bagMachine;
    }

    private static BagItemDto Map(BagItemEntity bagItemEntity)
    {
        var bagItem = new BagItemDto(bagItemEntity.Id, bagItemEntity.ExternalId, bagItemEntity.Count);
        return bagItem;
    }
}
