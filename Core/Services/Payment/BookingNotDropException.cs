namespace Core.Services.Payment;

public class BookingNotDropException : Exception
{
    public BookingNotDropException(string message, Exception innerException) : base(message, innerException)
    {
    }
}