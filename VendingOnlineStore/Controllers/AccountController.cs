using Core.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models.Account;

namespace VendingOnlineStore.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
    {
        var newUser = MapToNewUser(model);
        var registerResult = await _accountService.RegisterAsync(newUser);

        if (registerResult.Succeeded)
        {
            return SuccessRedirect();
        }

        foreach (var error in registerResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        var loginDetails = MapToLoginDetails(model);
        var loginResult = await _accountService.LoginAsync(loginDetails);

        if (loginResult.Succeeded)
        {
            return SuccessRedirect();
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return SuccessRedirect();
    }

    private static NewUser MapToNewUser(RegisterViewModel registerViewModel)
    {
        var newUser = new NewUser(
            registerViewModel.Login,
            registerViewModel.City,
            registerViewModel.Password
        );
        return newUser;
    }

    private static LoginDetails MapToLoginDetails(LoginViewModel loginViewModel)
    {
        var loginDetails = new LoginDetails(
            loginViewModel.Login,
            loginViewModel.Password
        );
        return loginDetails;
    }

    private RedirectToActionResult SuccessRedirect()
    {
        return RedirectToAction("Index", "Home");
    }
}