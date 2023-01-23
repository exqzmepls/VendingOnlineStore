namespace VendingOnlineStore.Models.Checkout;

public class ContentDetailsViewModel
{
    public ContentDetailsViewModel(Guid bagContentId, ItemViewModel item, int wantedCount, int suggestedCount,
        decimal? totalPrice)
    {
        BagContentId = bagContentId;
        Item = item;
        WantedCount = wantedCount;
        SuggestedCount = suggestedCount;
        TotalPrice = totalPrice;
    }

    public Guid BagContentId { get; }
    public ItemViewModel Item { get; }
    public int WantedCount { get; }
    public int SuggestedCount { get; }
    public decimal? TotalPrice { get; }
}