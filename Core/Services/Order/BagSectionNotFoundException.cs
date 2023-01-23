namespace Core.Services.Order;

public class BagSectionNotFoundException : Exception
{
    public BagSectionNotFoundException(string message) : base(message)
    {
    }
}