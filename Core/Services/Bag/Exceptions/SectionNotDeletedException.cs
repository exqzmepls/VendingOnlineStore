namespace Core.Services.Bag.Exceptions;

public class SectionNotDeletedException : Exception
{
    public SectionNotDeletedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}