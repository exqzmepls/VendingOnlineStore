namespace VendingOnlineStore.Models.Order;

public class OrderViewModel
{
    public OrderViewModel(Guid id, DateTime creationDateUtc, string status)
    {
        Id = id;
        CreationDateUtc = creationDateUtc;
        Status = status;
    }

    public Guid Id { get; }
    public DateTime CreationDateUtc { get; }
    public string Status { get; }
}