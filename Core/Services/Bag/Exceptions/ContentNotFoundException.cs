namespace Core.Services.Bag.Exceptions;

public class ContentNotFoundException : Exception
{
    public ContentNotFoundException(string message) : base(message)
    {
    }
}