using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Manage;

public class UpdateProfileViewModel
{
    [Required(AllowEmptyStrings = false)]
    public string City { get; set; } = null!;
}