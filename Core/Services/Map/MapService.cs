using static Core.DataSimulation.Data;

namespace Core.Services.Map;

public class MapService : IMapService
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