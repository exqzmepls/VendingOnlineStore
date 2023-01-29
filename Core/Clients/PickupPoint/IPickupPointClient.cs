namespace Core.Clients.PickupPoint;

public interface IPickupPointClient
{
    public Task<IReadOnlyCollection<PickupPointPresentation>> GetPresentationsAsync(
        IEnumerable<PickupPointContentsSpecification> specifications);

    public Task<PickupPointPresentation> GetPresentationAsync(
        PickupPointContentsSpecification specification);
}

public record PickupPointPresentation(PickupPoint PickupPoint, IReadOnlyCollection<PickupPointContent> Contents);

public record PickupPoint(string Id, string Description, string Address);

public record PickupPointContent(Item Item, int AvailableCount, decimal? Price);

public record Item(string Id, string Name, string Description, string PhotoLink);

public record PickupPointContentsSpecification(string PickupPointId, IEnumerable<string> ItemsIds);