using Infrastructure;
using BagContentEntity = Infrastructure.Models.BagContent;

namespace Core.Repositories.BagContent;

public class BagContentRepository : IBagContentRepository
{
    private readonly AppDbContext _appDbContext;

    public BagContentRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    /*public async Task<Guid?> AddAsync(NewBagItemDto newBagItem)
    {
        var newBagItemEntity = new BagContent
        {
            ItemId = newBagItem.ExternalId,
            BagSectionId = newBagItem.MachineId
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
    }*/

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bagContentEntity = await _appDbContext.BagContents.FindAsync(id);
        if (bagContentEntity == default)
        {
            return false;
        }

        _appDbContext.BagContents.Remove(bagContentEntity);
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

    public async Task<BagContentDetails?> GetOrDefaultAsync(Guid id)
    {
        var bagContentEntity = await _appDbContext.BagContents.FindAsync(id);
        if (bagContentEntity == default)
        {
            return default;
        }

        var bagItem = Map(bagContentEntity);
        return bagItem;
    }

    public async Task<BagContentDetails?> UpdateAsync(Guid id, BagContentUpdate update)
    {
        var bagContentEntity = await _appDbContext.BagContents.FindAsync(id);
        if (bagContentEntity == default)
        {
            return default;
        }

        bagContentEntity.Count = update.NewCount;
        try
        {
            await _appDbContext.SaveChangesAsync();
            var updatedBagItem = Map(bagContentEntity);
            return updatedBagItem;
        }
        catch
        {
            return default;
        }
    }

    private static BagContentDetails Map(BagContentEntity bagContentEntity)
    {
        var bagContentDetails = new BagContentDetails(
            bagContentEntity.Id,
            bagContentEntity.BagSectionId,
            bagContentEntity.ItemId,
            bagContentEntity.Count
        );
        return bagContentDetails;
    }
}