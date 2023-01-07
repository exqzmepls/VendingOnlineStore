namespace Core.Services.Vending.Dtos;

public class ItemMachine
{
    public ItemMachine(string id, string address, string distance, decimal itemPrice)
    {
        Id = id;
        Address = address;
        Distance = distance;
        ItemPrice = itemPrice;
    }

    public string Id { get; }

    public string Address { get; }

    public string Distance { get; }

    public decimal ItemPrice { get; }
}
