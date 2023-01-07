using Core.Repositories.BagItem.Dtos;
using Infrastructure;
using BagItemEntity = Infrastructure.Models.BagItem;

namespace Core.Repositories.BagItem;

public class BagItemRepository : IBagItemRepository
{
    private readonly AppDbContext _appDbContext;

    public BagItemRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Guid?> AddAsync(NewBagItemDto newBagItem)
    {
        var newBagItemEntity = new BagItemEntity
        {
            ExternalId = newBagItem.ExternalId,
            BagMachineId = newBagItem.MachineId
        };

        var addedEntityEntry = _appDbContext.Add(newBagItemEntity);
        try
        {
            await _appDbContext.SaveChangesAsync();
            return addedEntityEntry.Entity.Id;
        }
        catch
        {
            return default;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bagItemEntity = _appDbContext.BagItems.Find(id);
        if (bagItemEntity == default)
        {
            return false;
        }

        _appDbContext.BagItems.Remove(bagItemEntity);
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

    public async Task<BagItemDto?> GetOrDefaultAsync(Guid id)
    {
        var bagItemEntity = await _appDbContext.BagItems.FindAsync(id);
        if (bagItemEntity == default)
        {
            return default;
        }

        var bagItem = Map(bagItemEntity);
        return bagItem;
    }

    public async Task<BagItemDto?> UpdateAsync(Guid id, UpdatedBagItemDto updatedBagItemDto)
    {
        var bagItemEntity = await _appDbContext.BagItems.FindAsync(id);
        if (bagItemEntity == default)
        {
            return default;
        }

        bagItemEntity.Count = updatedBagItemDto.NewCount;
        try
        {
            await _appDbContext.SaveChangesAsync();
            var updatedBagItem = Map(bagItemEntity);
            return updatedBagItem;
        }
        catch
        {
            return default;
        }
    }

    private static BagItemDto Map(BagItemEntity bagItemEntity)
    {
        var bagItem = new BagItemDto(bagItemEntity.Id, bagItemEntity.BagMachineId, bagItemEntity.ExternalId, bagItemEntity.Count);
        return bagItem;
    }
}
