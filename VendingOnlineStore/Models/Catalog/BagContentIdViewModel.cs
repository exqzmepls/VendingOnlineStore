using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Catalog;

public class BagContentIdViewModel
{
    [Required]
    public int Index { get; set; }

    [Required]
    public int OptionIndex { get; set; }

    [Required]
    public Guid Id { get; set; }
}