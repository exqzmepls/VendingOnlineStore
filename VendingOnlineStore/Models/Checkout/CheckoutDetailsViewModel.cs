namespace VendingOnlineStore.Models.Checkout;

public class CheckoutDetailsViewModel
{
    public CheckoutDetailsViewModel(Guid bagSectionId, PickupPointViewModel pickupPoint,
        IReadOnlyCollection<ContentDetailsViewModel> contents, decimal? totalPrice)
    {
        BagSectionId = bagSectionId;
        PickupPoint = pickupPoint;
        Contents = contents;
        TotalPrice = totalPrice;
    }

    public Guid BagSectionId { get; }
    public PickupPointViewModel PickupPoint { get; }
    public IReadOnlyCollection<ContentDetailsViewModel> Contents { get; }
    public decimal? TotalPrice { get; }
}