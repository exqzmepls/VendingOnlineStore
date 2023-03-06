namespace PaymentWebhookApi.Contracts;

public class WebhookNotification
{
    public string Event { get; set; } = null!;

    public PaymentObject Object { get; set; } = null!;
}