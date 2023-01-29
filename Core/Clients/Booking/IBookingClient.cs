namespace Core.Clients.Booking;

public interface IBookingClient
{
    public Task<BookingDetails> CreateBookingAsync(NewBooking newBooking);

    public Task DropBookingAsync(string bookingId);

    public Task<int> ApproveBookingAsync(string bookingId);
}

public record BookingDetails(string Id, PickupPoint PickupPoint, IReadOnlyCollection<BookingContent> Contents);

public record PickupPoint(string Id, string Description, string Address, double Latitude, double Longitude);

public record BookingContent(Item Item, int Count, decimal Price);

public record Item(string Id, string Name, string Description, string PhotoLink);

public record NewBooking(string PickupPointId, IEnumerable<NewBookingContent> Contents);

public record NewBookingContent(string ItemId, int Count);