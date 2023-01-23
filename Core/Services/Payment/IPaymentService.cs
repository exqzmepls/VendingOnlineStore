namespace Core.Services.Payment;

public interface IPaymentService
{
    public void Succeeded(string paymentId);

    public Task Canceled(string paymentId);

    public Task WaitingForCapture(string paymentId);
}