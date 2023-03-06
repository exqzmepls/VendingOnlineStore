using Core.Clients.Vending;
using Core.Extensions;
using static Application.DataSimulation.Data;

namespace Application.DataSimulation;

internal class DummyVendingClient : IVendingClient
{
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
}