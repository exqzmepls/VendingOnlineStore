namespace VendingOnlineStore.Models.Bag;

public class BagContentViewModel
{
    public BagContentViewModel(Guid id, string name, string description, string photoLink, int availalbeCount, decimal? price, int count, decimal? totalPrice)
    {
        Id = id;
        Name = name;
        Description = description;
        PhotoLink = photoLink;
        AvailalbeCount = availalbeCount;
        Price = price;
        Count = count;
        TotalPrice = totalPrice;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string PhotoLink { get; }
    public int AvailalbeCount { get; }
    public decimal? Price { get; }
    public int Count { get; }
    public decimal? TotalPrice { get; }
}
