using Core.Extensions;
using static Infrastructure.DummyData;

namespace Core.Clients.Vending;

public class DummyVendingClient : IVendingClient
{
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