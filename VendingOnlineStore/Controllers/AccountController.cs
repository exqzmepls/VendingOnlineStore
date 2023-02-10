using Core.Services.Account;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models.Account;

namespace VendingOnlineStore.Controllers;

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
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in registerResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
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
}