using Core.Identity;
using Core.Repositories.User;

namespace Core.Services.Manage;

public class ManageService : IManageService
{
    private readonly IUserManager _userManager;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IUserRepository _userRepository;

    public ManageService(
        IUserManager userManager,
        IUserIdentityProvider userIdentityProvider,
        IUserRepository userRepository
    )
    {
        _userManager = userManager;
        _userIdentityProvider = userIdentityProvider;
        _userRepository = userRepository;
    }

    public async Task<Profile?> GetProfileOrDefaultAsync()
    {
        var user = await _userManager.GetUserOrDefaultAsync();
        if (user == default)
            return default;

        var profile = new Profile(
            user.Login,
            user.City
        );
        return profile;
    }

    public async Task<UpdateResult> UpdateProfileAsync(ProfileUpdate update)
    {
        try
        {
            var userId = _userIdentityProvider.GetUserIdentifier();
            await _userRepository.UpdateCityAsync(userId, update.City);
        }
        catch
        {
            return UpdateResult.FailedResult();
        }

        return UpdateResult.SuccessResult();
    }
}