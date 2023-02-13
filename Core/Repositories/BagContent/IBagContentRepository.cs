namespace Core.Repositories.BagContent;

public interface IBagContentRepository
{
    public Task<BagContentDetailsData?> GetOrDefaultAsync(Guid id);

    public Task<Guid> CreateAsync(NewBagContentData newBagContentData);

    public Task UpdateAsync(Guid id, BagContentUpdate update);

    public Task DeleteAsync(Guid id);
}

public record BagContentDetailsData(Guid Id, BagSectionBriefData Section, string ItemId, int Count);

public record BagSectionBriefData(Guid Id, Guid UserId, string PickupPointId);

public record BagContentUpdate(int NewCount);

public record NewBagContentData(Guid BagSectionId, string ItemId, int Count);