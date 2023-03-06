using Core.Identity;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using SignInResult = Core.Identity.SignInResult;

namespace VendingOnlineStore.Identity;

internal class MicrosoftSingInManager : ISignInManager<User>
{
    private readonly SignInManager<User> _signInManager;

    public MicrosoftSingInManager(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task SignInAsync(User user)
    {
        await _signInManager.SignInAsync(user, isPersistent: false);
    }

    public async Task<SignInResult> PasswordSignInAsync(string login, string password)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(login, password, false, false);
        return signInResult.Succeeded ? SignInResult.SuccessResult() : SignInResult.FailedResult();
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}