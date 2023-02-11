using Core.Identity;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace VendingOnlineStore.Identity;

public class MicrosoftUserManager : IUserManager
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MicrosoftUserManager(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserDetails?> GetUserAsync()
    {
        var principal = _httpContextAccessor.HttpContext!.User;
        var user = await _userManager.GetUserAsync(principal);
        if (user == default)
            return default;

        var userDetails = MapToUserDetails(user);
        return userDetails;
    }

    private static UserDetails MapToUserDetails(User user)
    {
        var userDetails = new UserDetails(
            user.Id,
            user.UserName!,
            user.City
        );
        return userDetails;
    }
}