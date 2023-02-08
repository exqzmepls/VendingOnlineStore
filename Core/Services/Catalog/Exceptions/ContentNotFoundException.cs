namespace Core.Services.Catalog.Exceptions;

public class ContentNotFoundException : Exception
{
    public ContentNotFoundException(string message) : base(message)
    {
    }
}