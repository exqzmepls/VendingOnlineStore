namespace VendingOnlineStore.Models.Catalog;

public class BagContentViewModel
{
    public BagContentViewModel(int index, int optionIndex, string itemId, string pickupPointId,
        BagEntranceViewModel? bagEntrance)
    {
        Index = index;
        OptionIndex = optionIndex;
        ItemId = itemId;
        PickupPointId = pickupPointId;
        BagEntrance = bagEntrance;
    }

    public int Index { get; }
    public int OptionIndex { get; }
    public string ItemId { get; }
    public string PickupPointId { get; }
    public BagEntranceViewModel? BagEntrance { get; }
}