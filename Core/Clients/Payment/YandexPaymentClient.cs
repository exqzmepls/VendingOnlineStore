using Yandex.Checkout.V3;
using PaymentObject = Yandex.Checkout.V3.Payment;

namespace Core.Clients.Payment;

public class YandexPaymentClient : IPaymentClient
{
    private readonly AsyncClient _client;

    public YandexPaymentClient()
    {
        var client = new Client("957383", "test_XeWOJBeeQewqgpPhdi64qbHg_QBhwcPy1XzwSRggHpk");
        _client = client.MakeAsync();
    }

    public Task<PaymentDetails> CreatePaymentAsync() => CreatePaymentAsync(100);

    public async Task<PaymentDetails> CreatePaymentAsync(decimal price)
    {
        var newPayment = new NewPayment
        {
            Description = "Test payment",
            Capture = true,
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

    private static PaymentDetails MapToPaymentDetails(PaymentObject payment)
    {
        var paymentDetails = new PaymentDetails(payment.Id, payment.Confirmation.ConfirmationUrl);
        return paymentDetails;
    }
}