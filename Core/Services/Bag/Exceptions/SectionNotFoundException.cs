namespace Core.Services.Bag.Exceptions;

public class SectionNotFoundException : Exception
{
    public SectionNotFoundException(string message) : base(message)
    {
    }
}