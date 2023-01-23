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

    public Task<IReadOnlyCollection<PickupPointPresentation>> GetPickupPointsPresentationsAsync(
        IEnumerable<PickupPointContentsSpecification> specifications);

    public Task<PickupPointPresentation> GetPickupPointPresentationAsync(
        PickupPointContentsSpecification specification);

    public Task<BookingDetails> CreateBookingAsync(NewBooking newBooking);
}

public record PickupPoint(string Id, string Description, string Address, double Latitude, double Longitude);

public record Slot(string Id, ItemDetails Item, decimal Price, int Count);

public record Item(string Id, string Name, string Description, string PhotoLink, decimal MinPrice);

public record ItemPickupPoint(string Id, string Address, double Latitude, double Longitude, decimal ItemPrice);

public record PickupPointPresentation(PickupPointDetails PickupPoint, IReadOnlyCollection<ContentDetails> Contents);

public record PickupPointDetails(string Id, string Description, string Address);

public record ContentDetails(ItemDetails Item, int AvailableCount, decimal? Price);

public record ItemDetails(string Id, string Name, string Description, string PhotoLink);

public record PickupPointContentsSpecification(string PickupPointId, IEnumerable<string> ItemsIds);

public record BookingDetails(string Id, PickupPoint PickupPoint, IReadOnlyCollection<BookingContent> Contents);

public record BookingContent(ItemDetails Item, int Count, decimal Price);

public record NewBooking(string PickupPointId, IEnumerable<NewBookingContent> Contents);

public record NewBookingContent(string ItemId, int Count);