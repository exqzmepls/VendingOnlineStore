using Core.Services.Map;
using static Application.DataSimulation.Data;

namespace Application.Services;

internal class MapService : IMapService
{
    public Coordinates GetDefaultLocation()
    {
        return new Coordinates(58.009535, 56.224404);
    }

    public IEnumerable<PickupPoint> GetPickupPoints()
    {
        return PickupPoints.Select(p =>
            new PickupPoint(p.Address, p.Description, new Coordinates(p.Latitude, p.Longitude)));
    }
}