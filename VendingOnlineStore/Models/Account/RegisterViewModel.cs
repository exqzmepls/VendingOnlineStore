using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Account;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Login")]
    public string Login { get; set; } = null!;

    [Required]
    [Display(Name = "City")]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;
}