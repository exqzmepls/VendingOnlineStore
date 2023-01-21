namespace Infrastructure.Models;

public class Order
{
    public Guid Id { get; set; }

    public DateTime CreationDateUtc { get; set; }

    public string BookingId { get; set; } = null!;

    public OrderStatus Status { get; set; }

    public decimal TotalPrice { get; set; }

    public Payment? Payment { get; set; }

    public OrderPickupPoint? OrderPickupPoint { get; set; }

    public IReadOnlyCollection<OrderContent>? OrderContents { get; set; }
}