namespace Core.Repositories.Order;

public interface IOrderRepository
{
    public IQueryable<OrderBriefData> GetAll();

    public OrderDetailsData? GetByIdOrDefault(Guid id);

    public Guid CreateOrder(NewOrderData newOrder);

    public void Update(Guid id, OrderUpdateData orderUpdate);
}

public record OrderBriefData
{
    public required Guid Id { get; init; }
    public required DateTime CreationDateUtc { get; init; }
    public required OrderStatusData Status { get; init; }
    public required string PaymentId { get; init; }
    public required string BookingId { get; init; }
}

public record OrderDetailsData
{
    public required Guid Id { get; init; }
    public required DateTime CreationDateUtc { get; init; }
    public required string BookingId { get; init; }
    public required OrderStatusData Status { get; init; }
    public required OrderPaymentData Payment { get; init; }
    public required int? ReleaseCode { get; init; }
    public required OrderPickupPointData PickupPoint { get; init; }
    public required IReadOnlyCollection<OrderContentData> Contents { get; init; }
    public required decimal TotalPrice { get; init; }
}

public enum OrderStatusData
{
    WaitingPayment,
    WaitingReceiving,
    PaymentOverdue,
    Received,
    ReceivingOverdue
}

public record OrderPaymentData
{
    public required string Id { get; init; }
    public required string Link { get; init; }
}

public record NewOrderData
{
    public required string BookingId { get; init; }
    public required OrderPaymentData Payment { get; init; }
    public required OrderPickupPointData PickupPoint { get; init; }
    public required IEnumerable<OrderContentData> Contents { get; init; }
    public required decimal TotalPrice { get; init; }
}

public record OrderPickupPointData
{
    public required string Id { get; init; }
    public required string Address { get; init; }
    public required string Description { get; init; }
}

public record OrderContentData
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string PhotoLink { get; init; }
    public required int Count { get; init; }
    public required decimal Price { get; init; }
}

public record OrderUpdateData
{
    public required OrderStatusData? NewStatus { get; init; }
    public required int? ReleaseCode { get; init; }
}