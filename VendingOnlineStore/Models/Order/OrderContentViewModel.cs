namespace VendingOnlineStore.Models.Order;

public class OrderContentViewModel
{
    public OrderContentViewModel(string name, string description, string photoLink, int count, decimal price)
    {
        Name = name;
        Description = description;
        PhotoLink = photoLink;
        Count = count;
        Price = price;
    }

    public string Name { get; }
    public string Description { get; }
    public string PhotoLink { get; }
    public int Count { get; }
    public decimal Price { get; }
}