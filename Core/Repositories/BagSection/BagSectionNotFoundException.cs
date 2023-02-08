namespace Core.Repositories.BagSection;

public class BagSectionNotFoundException : Exception
{
    public BagSectionNotFoundException(string message) : base(message)
    {
    }
}