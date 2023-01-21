namespace VendingOnlineStore.Models.Order;

public class OrderDetailsViewModel
{
    public OrderDetailsViewModel(DateTime creationDateUtc, string status, string paymentLink,
        OrderPickupPointViewModel pickupPoint, IEnumerable<OrderContentViewModel> contents, decimal totalPrice)
    {
        CreationDateUtc = creationDateUtc;
        Status = status;
        PaymentLink = paymentLink;
        PickupPoint = pickupPoint;
        Contents = contents;
        TotalPrice = totalPrice;
    }

    public DateTime CreationDateUtc { get; }
    public string Status { get; }
    public string PaymentLink { get; }
    public OrderPickupPointViewModel PickupPoint { get; }
    public IEnumerable<OrderContentViewModel> Contents { get; }
    public decimal TotalPrice { get; }
}