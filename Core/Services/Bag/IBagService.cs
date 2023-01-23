namespace Core.Services.Bag;

public interface IBagService
{
    public Task<IReadOnlyCollection<BagSection>> GetSectionsAsync();

    public Task<bool> RemoveContentAsync(Guid contentId);

    public Task<bool> IncreaseContentCountAsync(Guid contentId);

    public Task<bool> DecreaseContentCountAsync(Guid contentId);
}

public record BagSection(Guid Id, PickupPoint PickupPoint, IReadOnlyCollection<BagContent> Contents,
    decimal? TotalPrice);

public record PickupPoint(string Description, string Address);

public record BagContent(Guid Id, Item Item, int AvailableCount, decimal? Price, int Count, decimal? TotalPrice);

public record Item(string Name, string Description, string PhotoLink);