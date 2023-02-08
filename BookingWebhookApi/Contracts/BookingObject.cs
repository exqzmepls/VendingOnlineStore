using System.ComponentModel.DataAnnotations;

namespace BookingWebhookApi.Contracts;

public class BookingObject
{
    [Required]
    public string Id { get; set; } = null!;
}