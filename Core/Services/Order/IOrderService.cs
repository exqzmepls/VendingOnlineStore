namespace Core.Services.Order;

public interface IOrderService
{
    public IQueryable<OrderBrief> GetAll();

    public Task<string> CreateAsync(NewOrder newOrder);

    public OrderDetails? GetByIdOrDefault(Guid id);
}

public record OrderBrief
{
    public required Guid Id { get; init; }
    public required DateTime CreationDateUtc { get; init; }
    public required OrderStatus Status { get; init; }
}

public record OrderDetails
{
    public required Guid Id { get; init; }
    public required DateTime CreationDateUtc { get; init; }
    public required OrderStatus Status { get; init; }
    public required string PaymentLink { get; init; }
    public required int? ReleaseCode { get; init; }
    public required OrderPickupPoint PickupPoint { get; init; }
    public required IReadOnlyCollection<OrderContent> Contents { get; init; }
    public required decimal TotalPrice { get; init; }
}

public record OrderPickupPoint
{
    public required string Address { get; init; }
    public required string Description { get; init; }
}

public record OrderContent
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string PhotoLink { get; init; }
    public required int Count { get; init; }
    public required decimal Price { get; init; }
}

public record NewOrder
{
    public required Guid BagSectionId { get; init; }
    public required IReadOnlyCollection<NewOrderContent> Contents { get; init; }
}

public record NewOrderContent
{
    public required Guid BagContentId { get; init; }
    public required int Count { get; init; }
}

public enum OrderStatus
{
    WaitingPayment,
    WaitingReceiving,
    PaymentOverdue,
    Received,
    ReceivingOverdue
}