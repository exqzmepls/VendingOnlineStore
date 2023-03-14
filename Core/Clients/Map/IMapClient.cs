namespace Core.Clients.Map;

public interface IMapClient
{
    public Task<IReadOnlyCollection<CityBriefData>> GetCitiesAsync();

    public Task<CityDetailsData> GetCityAsync(int id);

    public Task<IReadOnlyCollection<PickupPointData>> GetPickupPointsAsync();
}

public record CityBriefData(int Id, string Name);

public record CityDetailsData(string Name, double Latitude, double Longitude);

public record PickupPointData(string Description, string Address, double Latitude, double Longitude);