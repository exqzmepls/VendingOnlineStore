namespace VendingOnlineStore.Clients.Vending.Dtos;

public class MachineItemInfo
{
    public MachineItemInfo(ItemInfo itemInfo, int availableCount, decimal? price)
    {
        ItemInfo = itemInfo;
        AvailableCount = availableCount;
        Price = price;
    }

    public ItemInfo ItemInfo { get; }
    public int AvailableCount { get; }
    public decimal? Price { get; }
}