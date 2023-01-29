namespace Core.Services.Payment.Exceptions;

public class BookingNotDropException : Exception
{
    public BookingNotDropException(string message, Exception innerException) : base(message, innerException)
    {
    }
}