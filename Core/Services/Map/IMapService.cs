namespace Core.Services.Map;

public interface IMapService
{
    public Task<Coordinates> GetDefaultLocationAsync();

    public Task<IEnumerable<PickupPoint>> GetPickupPointsAsync();
}

public record PickupPoint(string Address, string Description, Coordinates Coordinates);

public record Coordinates(double Latitude, double Longitude);