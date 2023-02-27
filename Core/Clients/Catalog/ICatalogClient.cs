namespace Core.Clients.Catalog;

public interface ICatalogClient
{
    public IReadOnlyCollection<Option> GetOptions(LocationData locationData);
}

public record Option(Item Item, IReadOnlyCollection<Content> Contents);

public record Item(string Id, string Name, string Description, string PhotoLink);

public record Content(PickupPoint PickupPoint, decimal Price);

public record PickupPoint(string Id, string Address, string Description);

public record LocationData(double Latitude, double Longitude, double Radius);