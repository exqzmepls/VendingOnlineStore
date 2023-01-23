namespace Core.Services.Payment;

public interface IPaymentService
{
    public Task ProcessEventAsync(string eventName, string paymentId);
}