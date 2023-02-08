namespace VendingOnlineStore.Models.Catalog;

public class BagEntranceViewModel
{
    public BagEntranceViewModel(Guid bagContentId, int count)
    {
        BagContentId = bagContentId;
        Count = count;
    }

    public Guid BagContentId { get; }
    public int Count { get; }
}