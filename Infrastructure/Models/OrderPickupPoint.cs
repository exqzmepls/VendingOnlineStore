namespace Infrastructure.Models;

public class OrderPickupPoint
{
    public Guid Id { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid OrderId { get; set; }

    public Order? Order { get; set; }
}