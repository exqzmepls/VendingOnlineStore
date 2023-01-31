using System.ComponentModel.DataAnnotations;

namespace BookingWebhookApi.Contracts;

public class OverdueNotification
{
    [Required]
    public BookingObject Booking { get; set; } = null!;
}