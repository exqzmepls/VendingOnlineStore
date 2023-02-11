using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Account;

public class LoginViewModel
{
    [Required]
    public string Login { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}