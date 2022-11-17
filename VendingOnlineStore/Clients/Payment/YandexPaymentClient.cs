using Yandex.Checkout.V3;

namespace VendingOnlineStore.Clients.Payment;

public class YandexPaymentClient : IPaymentClient
{
    private readonly AsyncClient _client;

    public YandexPaymentClient()
    {
        var client = new Client("957383", "");
        _client = client.MakeAsync();
    }

    public async Task<string> CreatePayment()
    {
        var newPayment = new NewPayment
        {
            Description = "Test payment",
            Capture = true,
            Amount = new Amount
            {
                Value = 100,
                Currency = "RUB"
            },
            Confirmation = new Confirmation
            {
                Type = ConfirmationType.Redirect,
                ReturnUrl = "https://localhost:7128"
            }
        };
        var payment = await _client.CreatePaymentAsync(newPayment);
        return payment.Confirmation.ConfirmationUrl;
    }
}
