namespace Core.Clients.Vending;

public interface IVendingClient
{
    [Obsolete("Not implemented")]
    public Task<IReadOnlyCollection<PickupPoint>> GetPickupPointsAsync();

    [Obsolete("Not implemented")]
    public Task<IReadOnlyCollection<Slot>> GetPickupPointSlotsAsync(string pickupPointId);

    public Task<IReadOnlyCollection<Item>> GetItemsAsync();

    public Task<Item?> GetItemAsync(string itemId);

    public Task<IReadOnlyCollection<ItemPickupPoint>> GetItemPickupPointsAsync(string itemId);
}

public record PickupPoint(string Id, string Description, string Address, double Latitude, double Longitude);

public record Slot(string Id, ItemDetails Item, decimal Price, int Count);

public record ItemDetails(string Id, string Name, string Description, string PhotoLink);

public record Item(string Id, string Name, string Description, string PhotoLink, decimal MinPrice);

public record ItemPickupPoint(string Id, string Address, double Latitude, double Longitude, decimal ItemPrice);