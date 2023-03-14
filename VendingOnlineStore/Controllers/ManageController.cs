using System.ComponentModel.DataAnnotations;
using Core.Services.Manage;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models.Manage;

namespace VendingOnlineStore.Controllers;

public class ManageController : Controller
{
    private readonly IManageService _manageService;

    public ManageController(IManageService manageService)
    {
        _manageService = manageService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Profile");
    }

    [HttpGet]
    public async Task<IActionResult> ProfileAsync()
    {
        var profile = await _manageService.GetProfileOrDefaultAsync();
        if (profile == default)
            return NotFound();

        var model = MapToProfileViewModel(profile);
        var cities = await _manageService.GetCitiesAsync();
        ViewBag.CityOptions = cities.Select(MapToCityOptionViewModel);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfileAsync([FromForm, Required] UpdateProfileViewModel form)
    {
        var profileUpdate = MapToProfileUpdate(form);
        var updateResult = await _manageService.UpdateProfileAsync(profileUpdate);

        TempData["status"] = updateResult.Succeeded
            ? "Your profile has been updated."
            : "Unexpected error when trying to set City.";
        return RedirectToAction("Profile");
    }

    private static ProfileViewModel MapToProfileViewModel(Profile profile)
    {
        var profileViewModel = new ProfileViewModel(
            profile.Login,
            profile.CityId
        );
        return profileViewModel;
    }

    private static ProfileUpdate MapToProfileUpdate(UpdateProfileViewModel updateProfileViewModel)
    {
        var profileUpdate = new ProfileUpdate(
            updateProfileViewModel.CityId
        );
        return profileUpdate;
    }

    private static CityOptionViewModel MapToCityOptionViewModel(City city)
    {
        var cityOptionViewModel = new CityOptionViewModel(
            city.Id,
            city.Name
        );
        return cityOptionViewModel;
    }
}