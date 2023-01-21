namespace Infrastructure.Models;

public class OrderContent
{
    public Guid Id { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PhotoLink { get; set; } = null!;

    public int Count { get; set; }

    public decimal Price { get; set; }

    public Guid OrderId { get; set; }

    public Order? Order { get; set; }
}