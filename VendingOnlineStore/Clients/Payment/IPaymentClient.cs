namespace VendingOnlineStore.Clients.Payment;

public interface IPaymentClient
{
    public Task<string> CreatePayment();

    public Task<string> CreatePayment(decimal price);
}
