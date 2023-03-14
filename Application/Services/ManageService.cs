using Core.Clients.Map;
using Core.Identity;
using Core.Repositories.User;
using Core.Services.Manage;

namespace Application.Services;

internal class ManageService : IManageService
{
    private readonly IMapClient _mapClient;
    private readonly IUserManager _userManager;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IUserRepository _userRepository;

    public ManageService(
        IMapClient mapClient,
        IUserManager userManager,
        IUserIdentityProvider userIdentityProvider,
        IUserRepository userRepository
    )
    {
        _mapClient = mapClient;
        _userManager = userManager;
        _userIdentityProvider = userIdentityProvider;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        var cities = await _mapClient.GetCitiesAsync();
        var result = cities.Select(city => new City(city.Id, city.Name));
        return result;
    }

    public async Task<Profile?> GetProfileOrDefaultAsync()
    {
        var user = await _userManager.GetUserOrDefaultAsync();
        if (user == default)
            return default;

        var profile = new Profile(
            user.Login,
            user.CityId
        );
        return profile;
    }

    public async Task<UpdateResult> UpdateProfileAsync(ProfileUpdate update)
    {
        try
        {
            var userId = _userIdentityProvider.GetUserIdentifier();
            await _userRepository.UpdateCityAsync(userId, update.CityId);
        }
        catch
        {
            return UpdateResult.FailedResult();
        }

        return UpdateResult.SuccessResult();
    }
}