using Application.DataSimulation;
using Core.Clients.Map;
using Core.Extensions;
using CoreServiceSdk;

namespace Application.Clients;

internal class MapClient : IMapClient
{
    private readonly HttpClient _httpClient;

    public MapClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<CityBriefData>> GetCitiesAsync()
    {
        return new CityBriefData[]
        {
            new CityBriefData(1, "Perm"),
            new CityBriefData(2, "Moscow")
        };

        var client = CreateClient();
        var cities = await client.GetAllCityAsync();

        // todo fix [][]
        var result = cities.First()
            .Select(city => new CityBriefData(city.Id!.Value, city.Name))
            .ToReadOnlyCollection();
        return result;
    }

    public async Task<CityDetailsData> GetCityAsync(int id)
    {
        return new CityDetailsData[]
        {
            new CityDetailsData("Perm", 58.010455, 56.229443),
            new CityDetailsData("Moscow", 55.755864, 37.617698)
        }[id - 1];

        var client = CreateClient();
        var city = await client.GetCityByIdAsync(id);
        var result = new CityDetailsData(city.Name, city.Latitude!.Value, city.Longitude!.Value);
        return result;
    }

    public async Task<IReadOnlyCollection<PickupPointData>> GetPickupPointsAsync()
    {
        return Data.PickupPoints
            .Select(p => new PickupPointData(p.Description, p.Address, p.Latitude, p.Longitude))
            .ToReadOnlyCollection();
        
        var client = CreateClient();
        var pickupPoints = await client.GetAllVendingAsync();
        var result = new List<PickupPointData>();
        foreach (var pickupPoint in pickupPoints.First())
        {
            var pickupPointAddress = await client.GetAddressByIdAsync(pickupPoint.Address_id!.Value);
            var address = $"{pickupPointAddress.Street} {pickupPointAddress.House}";
            var item = new PickupPointData(
                pickupPoint.Description,
                address,
                pickupPointAddress.Latitude!.Value,
                pickupPointAddress.Longitude!.Value
            );
            result.Add(item);
        }

        return result;
    }

    private Client CreateClient() => new(_httpClient);
}