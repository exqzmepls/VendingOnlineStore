namespace Core.Repositories.BagContent;

public class BagContentNotFoundException : Exception
{
    public BagContentNotFoundException(string message) : base(message)
    {
    }
}