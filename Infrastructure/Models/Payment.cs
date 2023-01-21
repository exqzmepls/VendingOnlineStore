namespace Infrastructure.Models;

public class Payment
{
    public Guid Id { get; set; }

    public string ExternalId { get; set; } = null!;

    public string Link { get; set; } = null!;

    public Guid OrderId { get; set; }

    public Order? Order { get; set; }
}