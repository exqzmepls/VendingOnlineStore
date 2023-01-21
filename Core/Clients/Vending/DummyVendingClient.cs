using Core.Extensions;

namespace Core.Clients.Vending;

public class DummyVendingClient : IVendingClient
{
    private static readonly IList<DummyPickupPoint> PickupPoints = new List<DummyPickupPoint>
    {
        new("m1", "hse 3", "бульвар Гагарина, 37А, Пермь", 58.016314, 56.276928),
        new("m2", "shopping mall", "улица Революции, 13, Пермь", 58.007545, 56.261786),
        new("m3", "hse 1", "Студенческая улица, 38, Пермь", 58.010662, 56.281509)
    };

    private static readonly IList<DummyItem> Items = new List<DummyItem>
    {
        new("i1", "water", "cool water",
            "https://www.topfreeshop.ru/4515-9179-large/distillirovannaja-voda.jpg"),
        new("i2", "chocolate", "cool chocolate",
            "https://alania-market.ru/image/cache/catalog/konditerskie-izdeliya/shokolad/7/61099454-1-640x640.jpg"),
        new("i3", "cookies", "cool cookies",
            "https://i.pinimg.com/originals/5d/d5/db/5dd5dbc145317cb6a9fc7fbdfcdcaba5.jpg")
    };

    private static readonly IList<DummyPickupPointContent> PickupPointsContents = new List<DummyPickupPointContent>
    {
        new("s1", PickupPoints[0], Items[0], 10, 10.99m), // 1
        new("s2", PickupPoints[0], Items[1], 15, 18.99m), // 2
        new("s3", PickupPoints[0], Items[2], 7, 25.99m), // 3
        new("s4", PickupPoints[1], Items[0], 3, 8.99m), // 1
        new("s5", PickupPoints[1], Items[1], 30, 35.99m), // 2
        new("s6", PickupPoints[1], Items[2], 13, 23.99m), // 3
        new("s7", PickupPoints[2], Items[0], 28, 17.99m), // 1
        new("s8", PickupPoints[2], Items[1], 24, 19.99m), // 2
        new("s9", PickupPoints[2], Items[2], 14, 22.99m) // 3
    };

    public async Task<Item?> GetItemAsync(string itemId)
    {
        await Task.Delay(35);
        var itemMinPrice = FindItemMinPrice(itemId);
        if (itemMinPrice == default)
            return default;

        var item = Items.Single(i => i.Id == itemId);
        var result = new Item(item.Id, item.Name, item.Description, item.PhotoLink, itemMinPrice.Value);
        return result;
    }

    public async Task<IReadOnlyCollection<ItemPickupPoint>> GetItemPickupPointsAsync(string itemId)
    {
        await Task.Delay(35);
        var result = new List<ItemPickupPoint>();
        foreach (var (_, pickupPoint, dummyItem, _, price) in PickupPointsContents)
        {
            if (dummyItem.Id != itemId)
                continue;
            result.Add(new ItemPickupPoint(pickupPoint.Id, pickupPoint.Address, pickupPoint.Latitude,
                pickupPoint.Longitude, price));
        }

        return result;
    }

    public async Task<IReadOnlyCollection<PickupPointPresentation>> GetPickupPointsPresentationsAsync(
        IEnumerable<PickupPointContentsSpecification> specifications)
    {
        await Task.Delay(35);
        var result = specifications.Select(specification =>
        {
            var pickupPoint = PickupPoints.Single(x => x.Id == specification.PickupPointId);

            var pickupPointDetails =
                new PickupPointDetails(pickupPoint.Id, pickupPoint.Description, pickupPoint.Address);

            var contents = specification.ItemsIds.Select(itemId =>
            {
                var item = Items.Single(i => i.Id == itemId);
                var itemDetails = new ItemDetails(item.Id, item.Name, item.Description, item.PhotoLink);

                var pickupPointContent = PickupPointsContents.SingleOrDefault(c =>
                    c.PickupPoint.Id == pickupPoint.Id && c.Item.Id == itemId);
                if (pickupPointContent == default)
                {
                    var noContentDetails = new ContentDetails(itemDetails, 0, default);
                    return noContentDetails;
                }

                var contentDetails = new ContentDetails(itemDetails, pickupPointContent.AvailableCount,
                    pickupPointContent.Price);
                return contentDetails;
            }).ToReadOnlyCollection();

            var machineItemsInfo = new PickupPointPresentation(pickupPointDetails, contents);
            return machineItemsInfo;
        }).ToReadOnlyCollection();
        return result;
    }

    public async Task<PickupPointPresentation> GetPickupPointPresentationAsync(
        PickupPointContentsSpecification specification)
    {
        var result = await GetPickupPointsPresentationsAsync(new[] { specification });
        return result.Single();
    }

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
            var item = new ItemDetails(dummyItem.Id, dummyItem.Name, dummyItem.Description, dummyItem.PhotoLink);
            var content = new BookingContent(item, c.Count, dummyContent.Price);
            return content;
        }).ToReadOnlyCollection();
        var booking = new BookingDetails(id, pickupPoint, contents);
        return booking;
    }

    public async Task<IReadOnlyCollection<Item>> GetItemsAsync()
    {
        await Task.Delay(35);
        var items = Items.Where(i => FindItemMinPrice(i.Id) != default).Select(i =>
        {
            var price = FindItemMinPrice(i.Id);
            var item = new Item(i.Id, i.Name, i.Description, i.PhotoLink, price!.Value);
            return item;
        }).ToReadOnlyCollection();
        return items;
    }

    public async Task<IReadOnlyCollection<PickupPoint>> GetPickupPointsAsync()
    {
        await Task.Delay(35);
        var result = PickupPoints
            .Select(p => new PickupPoint(p.Id, p.Description, p.Address, p.Latitude, p.Longitude))
            .ToReadOnlyCollection();
        return result;
    }

    public async Task<IReadOnlyCollection<Slot>> GetPickupPointSlotsAsync(string pickupPointId)
    {
        await Task.Delay(35);
        var result = PickupPointsContents.Select(c =>
        {
            var item = new ItemDetails(c.Item.Id, c.Item.Name, c.Item.Description, c.Item.PhotoLink);
            return new Slot(c.Id, item, c.Price, c.AvailableCount);
        }).ToReadOnlyCollection();
        return result;
    }

    private static decimal? FindItemMinPrice(string itemId)
    {
        var price = default(decimal?);
        foreach (var pickupPointContent in PickupPointsContents)
        {
            if (pickupPointContent.Item.Id != itemId)
                continue;
            if (price.HasValue)
            {
                if (pickupPointContent.Price < price.Value)
                {
                    price = pickupPointContent.Price;
                }
            }
            else
            {
                price = pickupPointContent.Price;
            }
        }

        return price;
    }
}

internal record DummyItem(string Id, string Name, string Description, string PhotoLink);

internal record DummyPickupPoint(string Id, string Description, string Address, double Latitude, double Longitude);

internal record DummyPickupPointContent(string Id, DummyPickupPoint PickupPoint, DummyItem Item, int AvailableCount,
    decimal Price);