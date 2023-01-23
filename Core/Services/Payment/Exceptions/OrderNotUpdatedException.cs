namespace Core.Services.Payment;

public class OrderNotUpdatedException : Exception
{
    public OrderNotUpdatedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}