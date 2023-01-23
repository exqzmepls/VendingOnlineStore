namespace VendingOnlineStore.Models.Checkout;

public class ItemViewModel
{
    public ItemViewModel(string name, string photoLink, int availableCount, decimal? price)
    {
        Name = name;
        PhotoLink = photoLink;
        AvailableCount = availableCount;
        Price = price;
    }

    public string Name { get; }
    public string PhotoLink { get; }
    public int AvailableCount { get; }
    public decimal? Price { get; }
}