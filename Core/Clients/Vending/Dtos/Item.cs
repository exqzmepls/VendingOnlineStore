namespace Core.Clients.Vending.Dtos;

public class Item
{
    public Item(string id, string name, string description, string photoLink, decimal minPrice)
    {
        Id = id;
        Name = name;
        Description = description;
        PhotoLink = photoLink;
        MinPrice = minPrice;
    }

    public string Id { get; }

    public string Name { get; }

    public string Description { get; }

    public string PhotoLink { get; }

    public decimal MinPrice { get; }
}
