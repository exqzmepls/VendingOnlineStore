using Core.Clients.Map;
using Core.Identity;
using Core.Services.Map;

namespace Application.Services;

internal class MapService : IMapService
{
    private readonly IUserManager _userManager;
    private readonly IMapClient _mapClient;

    public MapService(
        IUserManager userManager,
        IMapClient mapClient
    )
    {
        _userManager = userManager;
        _mapClient = mapClient;
    }

    public async Task<Coordinates> GetDefaultLocationAsync()
    {
        var user = await _userManager.GetUserOrDefaultAsync();
        var userCity = await _mapClient.GetCityAsync(user!.CityId);
        return new Coordinates(userCity.Latitude, userCity.Longitude);
    }

    public async Task<IEnumerable<PickupPoint>> GetPickupPointsAsync()
    {
        var pickupPoints = await _mapClient.GetPickupPointsAsync();
        var result = pickupPoints.Select(pickupPoint =>
        {
            var coordinates = new Coordinates(pickupPoint.Latitude, pickupPoint.Longitude);
            return new PickupPoint(pickupPoint.Address, pickupPoint.Description, coordinates);
        });
        return result;
    }
}