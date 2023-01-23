using Core.Extensions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using BagSectionEntity = Infrastructure.Models.BagSection;
using BagContentEntity = Infrastructure.Models.BagContent;

namespace Core.Repositories.BagSection;

public class BagSectionRepository : IBagSectionRepository
{
    private readonly AppDbContext _appDbContext;

    public BagSectionRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bagSectionEntity = await _appDbContext.BagSections.FindAsync(id);
        if (bagSectionEntity == default)
        {
            return false;
        }

        _appDbContext.BagSections.Remove(bagSectionEntity);
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

    public IQueryable<BagSectionDetails> GetAll()
    {
        var result = _appDbContext.BagSections
            .Include(s => s.BagContents)
            .Select(s => MapToSectionDetails(s));
        return result;
    }

    public async Task<BagSectionDetails?> GetByIdOrDefaultAsync(Guid id)
    {
        var bagSectionEntity = await _appDbContext.BagSections.FindAsync(id);
        if (bagSectionEntity == default)
        {
            return default;
        }

        await _appDbContext.Entry(bagSectionEntity).Collection(m => m.BagContents!).LoadAsync();
        var bagMachine = MapToSectionDetails(bagSectionEntity);
        return bagMachine;
    }

    private static BagSectionDetails MapToSectionDetails(BagSectionEntity bagSectionEntity)
    {
        var contents = bagSectionEntity.BagContents!
            .Select(MapToBagContentDetails)
            .ToReadOnlyCollection();
        var bagSectionDetails = new BagSectionDetails(
            bagSectionEntity.Id,
            bagSectionEntity.PickupPointId,
            contents
        );
        return bagSectionDetails;
    }

    private static BagContentBrief MapToBagContentDetails(BagContentEntity bagContentDetailsEntity)
    {
        var bagContentDetails = new BagContentBrief(
            bagContentDetailsEntity.Id,
            bagContentDetailsEntity.ItemId,
            bagContentDetailsEntity.Count
        );
        return bagContentDetails;
    }
}