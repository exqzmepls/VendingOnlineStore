namespace Core.Repositories.Order;

public interface IOrderRepository
{
    public IQueryable<OrderBrief> GetAll();

    public OrderDetails? GetByIdOrDefault(Guid id);

    public Guid CreateOrder(NewOrder newOrder);
}

public record OrderBrief(Guid Id, DateTime CreationDateUtc, Status Status);

public record OrderDetails(Guid Id, DateTime CreationDateUtc, string BookingId, Status Status, PaymentInfo Payment,
    PickupPointInfo PickupPoint, IReadOnlyCollection<ContentInfo> Contents, decimal TotalPrice);

public enum Status
{
    WaitingPayment,
    WaitingReceiving,
    PaymentOverdue,
    Received,
    ReceivingOverdue
}

public record PaymentInfo(string Id, string Link);

public record NewOrder(string BookingId, PaymentInfo Payment, PickupPointInfo PickupPoint,
    IEnumerable<ContentInfo> Contents, decimal TotalPrice);

public record PickupPointInfo(string Id, string Address, string Description);

public record ContentInfo(string Id, string Name, string Description, string PhotoLink, int Count, decimal Price);