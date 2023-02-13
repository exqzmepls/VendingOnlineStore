using Infrastructure;
using BagContentEntity = Infrastructure.Models.BagContent;
using BagSectionEntity = Infrastructure.Models.BagSection;

namespace Core.Repositories.BagContent;

public class BagContentRepository : IBagContentRepository
{
    private readonly AppDbContext _appDbContext;

    public BagContentRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Guid> CreateAsync(NewBagContentData newBagContentData)
    {
        var bagContent = new BagContentEntity
        {
            ItemId = newBagContentData.ItemId,
            Count = newBagContentData.Count,
            BagSectionId = newBagContentData.BagSectionId
        };

        var entry = _appDbContext.BagContents.Add(bagContent);
        try
        {
            await _appDbContext.SaveChangesAsync();
            return entry.Entity.Id;
        }
        catch (Exception exception)
        {
            throw new DbException("Content not created", exception);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var bagContentEntity = await _appDbContext.BagContents.FindAsync(id);
        if (bagContentEntity == default)
        {
            throw new BagContentNotFoundException("Content does not exist");
        }

        _appDbContext.BagContents.Remove(bagContentEntity);
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new DbException("Content delete fell", exception);
        }
    }

    public async Task<BagContentDetailsData?> GetOrDefaultAsync(Guid id)
    {
        var bagContentEntity = await _appDbContext.BagContents.FindAsync(id);
        if (bagContentEntity == default)
        {
            return default;
        }

        await _appDbContext.Entry(bagContentEntity).Reference(c => c.BagSection).LoadAsync();

        var bagContentDetailsData = MapToBagContentDetailsData(bagContentEntity);
        return bagContentDetailsData;
    }

    public async Task UpdateAsync(Guid id, BagContentUpdate update)
    {
        var bagContentEntity = await _appDbContext.BagContents.FindAsync(id);
        if (bagContentEntity == default)
        {
            throw new BagContentNotFoundException("Content does not exist");
        }

        bagContentEntity.Count = update.NewCount;
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new DbException("Content not updated", exception);
        }
    }

    private static BagContentDetailsData MapToBagContentDetailsData(BagContentEntity bagContentEntity)
    {
        var section = MapToBagSectionBriefData(bagContentEntity.BagSection!);
        var bagContentDetailsData = new BagContentDetailsData(
            bagContentEntity.Id,
            section,
            bagContentEntity.ItemId,
            bagContentEntity.Count
        );
        return bagContentDetailsData;
    }

    private static BagSectionBriefData MapToBagSectionBriefData(BagSectionEntity bagSectionEntity)
    {
        var bagSectionBriefData = new BagSectionBriefData(
            bagSectionEntity.Id,
            bagSectionEntity.UserId,
            bagSectionEntity.PickupPointId
        );
        return bagSectionBriefData;
    }
}