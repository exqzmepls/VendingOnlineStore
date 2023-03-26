using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Order;

public class OrderViewModel
{
    public required Guid Id { get; init; }
    public required DateTime CreationDateUtc { get; init; }
    public required OrderStatus Status { get; init; }

    [Display(Name = "Pickup Point")]
    public required string PickupPointAddress { get; init; }

    [Display(Name = "Total Price")]
    public required decimal TotalPrice { get; init; }
}