namespace Core.Repositories.Order;

public class OrderDto
{
    public OrderDto(Guid id, string paymentId, string bookingId)
    {
        Id = id;
        PaymentId = paymentId;
        BookingId = bookingId;
    }

    public Guid Id { get; }
    public string PaymentId { get; }
    public string BookingId { get; }
}