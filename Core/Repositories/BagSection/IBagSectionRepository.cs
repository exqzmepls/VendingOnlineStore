namespace Core.Repositories.BagSection;

public interface IBagSectionRepository
{
    public IQueryable<BagSectionDetails> GetAll();

    public Task<BagSectionDetails?> GetByIdOrDefaultAsync(Guid id);

    public Task<bool> DeleteAsync(Guid id);
}

public record BagSectionDetails(Guid Id, string PickupPointId, IReadOnlyCollection<BagContentBrief> Contents);

public record BagContentBrief(Guid Id, string ItemId, int Count);