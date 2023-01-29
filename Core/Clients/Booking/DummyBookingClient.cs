using Core.Extensions;
using static Infrastructure.DummyData;

namespace Core.Clients.Booking;

public class DummyBookingClient : IBookingClient
{
    public async Task<BookingDetails> CreateBookingAsync(NewBooking newBooking)
    {
        await Task.Delay(35);
        var id = Guid.NewGuid().ToString();
        var dummyPickupPoint = PickupPoints.Single(p => p.Id == newBooking.PickupPointId);
        var pickupPoint = new PickupPoint(dummyPickupPoint.Id, dummyPickupPoint.Description, dummyPickupPoint.Address,
            dummyPickupPoint.Latitude, dummyPickupPoint.Longitude);
        var contents = newBooking.Contents.Select(c =>
        {
            var dummyContent = PickupPointsContents.Single(d => d.PickupPoint.Id == pickupPoint.Id
                                                                && d.Item.Id == c.ItemId);
            var dummyItem = dummyContent.Item;
            var item = new Item(dummyItem.Id, dummyItem.Name, dummyItem.Description, dummyItem.PhotoLink);
            var content = new BookingContent(item, c.Count, dummyContent.Price);
            return content;
        }).ToReadOnlyCollection();
        var booking = new BookingDetails(id, pickupPoint, contents);
        return booking;
    }

    public async Task DropBookingAsync(string bookingId)
    {
        await Task.Delay(35);
    }

    public async Task<int> ApproveBookingAsync(string bookingId)
    {
        await Task.Delay(35);
        return Random.Shared.Next(1000, 10000);
    }
}