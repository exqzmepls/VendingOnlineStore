namespace VendingOnlineStore.Models.Order;

public class OrderDetailsViewModel
{
    public OrderDetailsViewModel(Guid id, DateTime creationDateUtc, OrderStatus status, string paymentLink,
        int? releaseCode, OrderPickupPointViewModel pickupPoint, IEnumerable<OrderContentViewModel> contents,
        decimal totalPrice)
    {
        Id = id;
        CreationDateUtc = creationDateUtc;
        Status = status;
        PaymentLink = paymentLink;
        ReleaseCode = releaseCode;
        PickupPoint = pickupPoint;
        Contents = contents;
        TotalPrice = totalPrice;
    }

    public Guid Id { get; }
    public DateTime CreationDateUtc { get; }
    public OrderStatus Status { get; }
    public string PaymentLink { get; }
    public int? ReleaseCode { get; }
    public OrderPickupPointViewModel PickupPoint { get; }
    public IEnumerable<OrderContentViewModel> Contents { get; }
    public decimal TotalPrice { get; }
}