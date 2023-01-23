namespace Core.Clients.Payment;

public interface IPaymentClient
{
    public Task<PaymentDetails> CreatePaymentAsync();

    public Task<PaymentDetails> CreatePaymentAsync(decimal price);
}

public record PaymentDetails(string Id, string Link);