namespace VendingOnlineStore.Services.Bag.Dtos;

public class ItemInfo
{
    public ItemInfo(string name, string description, string photoLink)
    {
        Name = name;
        Description = description;
        PhotoLink = photoLink;
    }

    public string Name { get; }
    public string Description { get; }
    public string PhotoLink { get; }
}