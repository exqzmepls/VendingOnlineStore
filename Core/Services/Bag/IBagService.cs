namespace Core.Services.Bag;

public interface IBagService
{
    public Task<IReadOnlyCollection<BagSection>> GetSectionsAsync();

    public Task RemoveContentAsync(Guid contentId);

    public Task IncreaseContentCountAsync(Guid contentId);

    public Task DecreaseContentCountAsync(Guid contentId);
}

public record BagSection(Guid Id, PickupPoint PickupPoint, IReadOnlyCollection<BagContent> Contents,
    decimal? TotalPrice);

public record PickupPoint(string Description, string Address);

public record BagContent(Guid Id, Item Item, int AvailableCount, decimal? Price, int Count, decimal? TotalPrice);

public record Item(string Name, string Description, string PhotoLink);