namespace Core.Repositories.BagContent;

public interface IBagContentRepository
{
    public Task<BagContentDetails?> GetOrDefaultAsync(Guid id);

    public Task<BagContentDetails?> UpdateAsync(Guid id, BagContentUpdate update);

    public Task<bool> DeleteAsync(Guid id);
}

public record BagContentDetails(Guid Id, Guid SectionId, string ItemId, int Count);

public record BagContentUpdate(int NewCount);