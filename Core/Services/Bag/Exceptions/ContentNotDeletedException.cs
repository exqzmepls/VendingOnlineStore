namespace Core.Services.Bag.Exceptions;

public class ContentNotDeletedException : Exception
{
    public ContentNotDeletedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}