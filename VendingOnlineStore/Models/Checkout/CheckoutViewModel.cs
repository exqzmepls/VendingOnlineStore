using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Checkout;

public class CheckoutViewModel
{
    [Required]
    public Guid BagSectionId { get; set; }
}