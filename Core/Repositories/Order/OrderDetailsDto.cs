namespace Core.Repositories.Order;

public class OrderDetailsDto
{
    public OrderDetailsDto(Guid id, string paymentId, string bookingId, Status status, int? obtainingCode)
    {
        Id = id;
        PaymentId = paymentId;
        BookingId = bookingId;
        Status = status;
        ObtainingCode = obtainingCode;
    }

    public Guid Id { get; }
    public string PaymentId { get; }
    public string BookingId { get; }
    public Status Status { get; }
    public int? ObtainingCode { get; }
}

public enum Status
{
    WaitingPayment, 
    Paid,
    NotPaid, // оплата просрочена
    // выдан
    // выдача просрочена
}