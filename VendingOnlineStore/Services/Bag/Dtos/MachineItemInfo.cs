namespace VendingOnlineStore.Services.Bag.Dtos;

public class MachineItemInfo
{
    public MachineItemInfo(int availableCount, decimal? price)
    {
        AvailableCount = availableCount;
        Price = price;
    }

    public int AvailableCount { get; }
    public decimal? Price { get; }
}