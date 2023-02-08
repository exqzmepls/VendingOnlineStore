namespace Core.Clients.Catalog;

public interface ICatalogClient
{
    public IReadOnlyCollection<Option> GetOptions(string city);

    public IReadOnlyCollection<Option> GetOptions(Place place);
}

public record Option(Item Item, IReadOnlyCollection<Content> Contents);

public record Item(string Id, string Name, string Description, string PhotoLink);

public record Content(PickupPoint PickupPoint, decimal Price);

public record PickupPoint(string Id, string Address, string Description);

public record Place(double Latitude, double Longitude, double Radius);