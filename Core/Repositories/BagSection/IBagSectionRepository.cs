namespace Core.Repositories.BagSection;

public interface IBagSectionRepository
{
    public IQueryable<BagSectionDetailsData> GetAll();

    public Task<BagSectionBriefData> CreateAsync(NewBagSectionData newBagSection);

    public Task<BagSectionDetailsData?> GetByIdOrDefaultAsync(Guid id);

    public Task DeleteAsync(Guid id);
}

public record BagSectionDetailsData
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string PickupPointId { get; init; }
    public required IReadOnlyCollection<BagContentBriefData> Contents { get; init; }
}

public record BagContentBriefData
{
    public required Guid Id { get; init; }
    public required string ItemId { get; init; }
    public required int Count { get; init; }
}

public record NewBagSectionData(Guid UserId, string PickupPointId, IEnumerable<NewBagSectionContentData> Contents);

public record NewBagSectionContentData(string ItemId, int Count);

public record BagSectionBriefData(Guid Id, IReadOnlyCollection<Guid> ContentsIds);