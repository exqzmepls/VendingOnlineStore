using Core.AppInterfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using SignInResult = Core.AppInterfaces.SignInResult;

namespace VendingOnlineStore.Implementations;

public class NonPersistentSingInManager : ISignInManager<User>
{
    private readonly SignInManager<User> _signInManager;

    public NonPersistentSingInManager(SignInManager<User> signInManager)
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
}