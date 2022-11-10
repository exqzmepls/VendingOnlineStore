namespace VendingOnlineStore.Services.Vending.Dtos;

public class MachineSlot
{
    public MachineSlot(string id, string itemName, string itemDescription, string itemPhotoLink, decimal price, int count)
    {
        Id = id;
        ItemName = itemName;
        ItemDescription = itemDescription;
        ItemPhotoLink = itemPhotoLink;
        Price = price;
        Count = count;
    }

    public string Id { get; }

    public string ItemName { get; }

    public string ItemDescription { get; }

    public string ItemPhotoLink { get; }

    public decimal Price { get; }

    public int Count { get; }
}
