namespace VendingOnlineStore.Services.Bag.Dtos;

public class BagItem
{
    public BagItem(Guid id, ItemInfo itemInfo, MachineItemInfo machineItemInfo, int count, decimal? totalPrice)
    {
        Id = id;
        ItemInfo = itemInfo;
        MachineItemInfo = machineItemInfo;
        Count = count;
        TotalPrice = totalPrice;
    }

    public Guid Id { get; }
    public ItemInfo ItemInfo { get; }
    public MachineItemInfo MachineItemInfo { get; }
    public int Count { get; }
    public decimal? TotalPrice { get; }
}
