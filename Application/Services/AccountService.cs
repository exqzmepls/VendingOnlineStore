using Core.Clients.Map;
using Core.Identity;
using Core.Services.Account;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Services;

internal class AccountService : IAccountService
{
    private readonly IMapClient _mapClient;
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly ISignInManager<User> _signInManager;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IMapClient mapClient,
        UserManager<User> userManager,
        IUserStore<User> userStore,
        ISignInManager<User> signInManager,
        ILogger<AccountService> logger
    )
    {
        _mapClient = mapClient;
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        var cities = await _mapClient.GetCitiesAsync();
        var result = cities.Select(city => new City(city.Id, city.Name));
        return result;
    }

    public async Task<RegisterResult> RegisterAsync(NewUser newUser)
    {
        var user = CreateUser(newUser.CityId);

        await _userStore.SetUserNameAsync(user, newUser.Login, CancellationToken.None);
        var identityResult = await _userManager.CreateAsync(user, newUser.Password);

        if (identityResult.Succeeded)
        {
            _logger.LogInformation("User created a new account with password");

            await _signInManager.SignInAsync(user);

            return RegisterResult.SuccessResult();
        }

        var errors = identityResult.Errors.Select(e => new RegisterError(e.Description));
        return RegisterResult.FailedResult(errors);
    }

    public async Task<LoginResult> LoginAsync(LoginDetails loginDetails)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(loginDetails.Login, loginDetails.Password);

        if (!signInResult.Succeeded)
            return LoginResult.FailedResult();

        _logger.LogInformation("User logged in");
        return LoginResult.SuccessResult();
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
    }

    private static User CreateUser(int cityId)
    {
        try
        {
            var user = Activator.CreateInstance<User>();
            user.CityExternalId = cityId;
            return user;
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'");
        }
    }
}