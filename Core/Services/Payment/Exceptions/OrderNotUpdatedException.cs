namespace Core.Services.Payment.Exceptions;

public class OrderNotUpdatedException : Exception
{
    public OrderNotUpdatedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}