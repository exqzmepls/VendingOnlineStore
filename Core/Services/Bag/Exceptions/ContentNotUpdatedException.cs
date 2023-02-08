namespace Core.Services.Bag.Exceptions;

public class ContentNotUpdatedException : Exception
{
    public ContentNotUpdatedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}