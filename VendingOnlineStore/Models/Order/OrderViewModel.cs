namespace VendingOnlineStore.Models.Order;

public class OrderViewModel
{
    public required Guid Id { get; init; }
    public required DateTime CreationDateUtc { get; init; }
    public required OrderStatus Status { get; init; }
}