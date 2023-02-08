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

    public async Task<BagSectionBriefData> CreateAsync(NewBagSectionData newBagSection)
    {
        var bagContents = newBagSection.Contents
            .Select(c => new BagContentEntity
            {
                ItemId = c.ItemId,
                Count = c.Count
            })
            .ToReadOnlyCollection();
        var bagSection = new BagSectionEntity
        {
            PickupPointId = newBagSection.PickupPointId,
            BagContents = bagContents
        };
        var entry = await _appDbContext.BagSections.AddAsync(bagSection);

        try
        {
            await _appDbContext.SaveChangesAsync();
            var entity = entry.Entity;
            var bagSectionId = entity.Id;
            var contentsIds = entity.BagContents!.Select(c => c.Id).ToReadOnlyCollection();
            var bagSectionBriefData = new BagSectionBriefData(bagSectionId, contentsIds);
            return bagSectionBriefData;
        }
        catch (Exception exception)
        {
            throw new DbException("Bag section not created", exception);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var bagSectionEntity = await _appDbContext.BagSections.FindAsync(id);
        if (bagSectionEntity == default)
        {
            throw new BagSectionNotFoundException("Section does not exist");
        }

        _appDbContext.BagSections.Remove(bagSectionEntity);
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new DbException("Section delete fell", exception);
        }
    }

    public IQueryable<BagSectionDetailsData> GetAll()
    {
        var result = _appDbContext.BagSections
            .Include(s => s.BagContents)
            .Select(bagSection => new BagSectionDetailsData
            {
                Id = bagSection.Id,
                PickupPointId = bagSection.PickupPointId,
                Contents = bagSection.BagContents!.Select(bagContent => new BagContentBriefData
                    {
                        Id = bagContent.Id,
                        ItemId = bagContent.ItemId,
                        Count = bagContent.Count
                    })
                    .ToReadOnlyCollection()
            });
        return result;
    }

    public async Task<BagSectionDetailsData?> GetByIdOrDefaultAsync(Guid id)
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

    private static BagSectionDetailsData MapToSectionDetails(BagSectionEntity bagSectionEntity)
    {
        var contents = bagSectionEntity.BagContents!
            .Select(MapToBagContentDetails)
            .ToReadOnlyCollection();
        var bagSectionDetailsData = new BagSectionDetailsData
        {
            Id = bagSectionEntity.Id,
            PickupPointId = bagSectionEntity.PickupPointId,
            Contents = contents
        };
        return bagSectionDetailsData;
    }

    private static BagContentBriefData MapToBagContentDetails(BagContentEntity bagContentDetailsEntity)
    {
        var bagContentDetailsData = new BagContentBriefData
        {
            Id = bagContentDetailsEntity.Id,
            ItemId = bagContentDetailsEntity.ItemId,
            Count = bagContentDetailsEntity.Count
        };
        return bagContentDetailsData;
    }
}