namespace VendingOnlineStore.Models.Catalog;

public class ContentViewModel
{
    public ContentViewModel(int optionIndex, PickupPointViewModel pickupPoint, decimal price,
        BagContentViewModel bagContent)
    {
        OptionIndex = optionIndex;
        PickupPoint = pickupPoint;
        Price = price;
        BagContent = bagContent;
    }

    public int OptionIndex { get; }
    public PickupPointViewModel PickupPoint { get; }
    public decimal Price { get; }
    public BagContentViewModel BagContent { get; }
}