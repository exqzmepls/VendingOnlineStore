namespace VendingOnlineStore.Clients.Vending.Dtos;

public class ItemInfo
{
    public ItemInfo(string id, string name, string description, string photoLink)
    {
        Id = id;
        Name = name;
        Description = description;
        PhotoLink = photoLink;
    }

    public string Id { get; }

    public string Name { get; }

    public string Description { get; }

    public string PhotoLink { get; }
}
