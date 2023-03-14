using Core.Clients.Payment;
using Microsoft.Extensions.Configuration;
using Yandex.Checkout.V3;
using PaymentObject = Yandex.Checkout.V3.Payment;

namespace Application.Clients;

internal class YandexPaymentClient : IPaymentClient
{
    private readonly AsyncClient _client;

    public YandexPaymentClient(IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection("YooKassa");
        var shopIdSection = configurationSection.GetRequiredSection("ShopId");
        var secretKeySection = configurationSection.GetRequiredSection("SecretKey");
        var client = new Client(shopIdSection.Value, secretKeySection.Value);
        _client = client.MakeAsync();
    }

    public Task<PaymentDetails> CreatePaymentAsync() => CreatePaymentAsync(100);

    public async Task<PaymentDetails> CreatePaymentAsync(decimal price)
    {
        var newPayment = new NewPayment
        {
            Description = "Test payment",
            Capture = false,
            Amount = new Amount
            {
                Value = price,
                Currency = "RUB"
            },
            Confirmation = new Confirmation
            {
                Type = ConfirmationType.Redirect,
                ReturnUrl = "https://localhost:7128"
            }
        };
        var payment = await _client.CreatePaymentAsync(newPayment);

        var result = MapToPaymentDetails(payment);
        return result;
    }

    public async Task CapturePaymentAsync(string paymentId)
    {
        await _client.CapturePaymentAsync(paymentId);
    }

    public async Task CancelPaymentAsync(string id)
    {
        await _client.CancelPaymentAsync(id);
    }

    private static PaymentDetails MapToPaymentDetails(PaymentObject payment)
    {
        var paymentDetails = new PaymentDetails(payment.Id, payment.Confirmation.ConfirmationUrl);
        return paymentDetails;
    }
}