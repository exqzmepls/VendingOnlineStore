using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Catalog;

public class NewBagContentViewModel
{
    [Required]
    public int Index { get; set; }

    [Required]
    public int OptionIndex { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string ItemId { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string PickupPointId { get; set; } = null!;
}