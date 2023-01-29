namespace Core.Services.Payment.Exceptions;

public class BookingNotApprovedException : Exception
{
    public BookingNotApprovedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}