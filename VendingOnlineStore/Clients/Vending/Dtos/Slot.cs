namespace VendingOnlineStore.Clients.Vending.Dtos;

public class Slot
{
    public Slot(string id, Item item, decimal price, int count)
    {
        Id = id;
        Item = item;
        Price = price;
        Count = count;
    }

    public string Id { get; }

    public Item Item { get; }

    public decimal Price { get; }

    public int Count { get; }
}
