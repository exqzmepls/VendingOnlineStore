namespace Core.Services.Catalog.Exceptions;

public class SectionNotFoundException : Exception
{
    public SectionNotFoundException(string message) : base(message)
    {
    }
}