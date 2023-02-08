using System.ComponentModel.DataAnnotations;

namespace BookingWebhookApi.Contracts;

public class ReleaseNotification
{
    [Required]
    public BookingObject Booking { get; set; } = null!;
}