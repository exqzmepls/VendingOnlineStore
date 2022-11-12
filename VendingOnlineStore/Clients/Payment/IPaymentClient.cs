namespace VendingOnlineStore.Clients.Payment;

public interface IPaymentClient
{
    public Task<string> CreatePayment();
}
