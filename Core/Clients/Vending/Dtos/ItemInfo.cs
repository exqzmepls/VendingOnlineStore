namespace Core.Clients.Vending.Dtos;

public class ItemInfo
{
    public ItemInfo(string externalId, string name, string description, string photoLink)
    {
        ExternalId = externalId;
        Name = name;
        Description = description;
        PhotoLink = photoLink;
    }

    public string ExternalId { get; }

    public string Name { get; }

    public string Description { get; }

    public string PhotoLink { get; }
}
