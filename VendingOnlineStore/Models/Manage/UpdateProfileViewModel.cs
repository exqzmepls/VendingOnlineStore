using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Manage;

public class UpdateProfileViewModel
{
    [Required]
    [Display(Name = "City")]
    public int CityId { get; set; }
}