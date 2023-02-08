namespace Core.Services.Booking;

public interface IBookingService
{
    public Task OnReleaseAsync(string bookingId);

    public Task OnOverdueAsync(string bookingId);
}