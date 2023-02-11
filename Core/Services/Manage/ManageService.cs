using Core.Identity;
using Core.Repositories.User;

namespace Core.Services.Manage;

public class ManageService : IManageService
{
    private readonly IUserManager _userManager;
    private readonly IUserRepository _userRepository;

    public ManageService(IUserManager userManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<Profile?> GetProfileOrDefaultAsync()
    {
        var user = await _userManager.GetUserAsync();
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
        var user = await _userManager.GetUserAsync();
        if (user == default)
            return UpdateResult.FailedResult();

        var userId = user.id;
        try
        {
            await _userRepository.UpdateCityAsync(userId, update.City);
        }
        catch
        {
            return UpdateResult.FailedResult();
        }

        return UpdateResult.SuccessResult();
    }
}