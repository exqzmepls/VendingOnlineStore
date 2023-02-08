using Core.Clients.PickupPoint;
using Core.Extensions;
using static Core.DataSimulation.Data;

namespace Core.DataSimulation;

public class DummyPickupPointClient : IPickupPointClient
{
    public async Task<IReadOnlyCollection<PickupPointPresentation>> GetPresentationsAsync(
        IEnumerable<PickupPointContentsSpecification> specifications)
    {
        await Task.Delay(35);
        var result = specifications.Select(specification =>
        {
            var pickupPoint = PickupPoints.Single(x => x.Id == specification.PickupPointId);

            var pickupPointDetails =
                new PickupPoint(pickupPoint.Id, pickupPoint.Description, pickupPoint.Address);

            var contents = specification.ItemsIds.Select(itemId =>
            {
                var item = Items.Single(i => i.Id == itemId);
                var itemDetails = new Item(item.Id, item.Name, item.Description, item.PhotoLink);

                var pickupPointContent = PickupPointsContents.SingleOrDefault(c =>
                    c.PickupPoint.Id == pickupPoint.Id && c.Item.Id == itemId);
                if (pickupPointContent == default)
                {
                    var noContentDetails = new PickupPointContent(itemDetails, 0, default);
                    return noContentDetails;
                }

                var contentDetails = new PickupPointContent(itemDetails, pickupPointContent.AvailableCount,
                    pickupPointContent.Price);
                return contentDetails;
            }).ToReadOnlyCollection();

            var machineItemsInfo = new PickupPointPresentation(pickupPointDetails, contents);
            return machineItemsInfo;
        }).ToReadOnlyCollection();
        return result;
    }

    public async Task<PickupPointPresentation> GetPresentationAsync(PickupPointContentsSpecification specification)
    {
        var result = await GetPresentationsAsync(new[] { specification });
        return result.Single();
    }
}