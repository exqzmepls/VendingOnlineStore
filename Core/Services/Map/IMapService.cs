namespace Core.Services.Map;

public interface IMapService
{
    public Coordinates GetDefaultLocation();

    public IEnumerable<PickupPoint> GetPickupPoints();
}

public record PickupPoint(string Address, string Description, Coordinates Coordinates);

public record Coordinates(double Latitude, double Longitude);