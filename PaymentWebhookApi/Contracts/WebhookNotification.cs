namespace PaymentWebhookApi.Contracts;

public class WebhookNotification
{
    public string Event { get; set; }

    public PaymentObject Object { get; set; }
}