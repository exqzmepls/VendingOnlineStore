using Core.AppInterfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

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
}