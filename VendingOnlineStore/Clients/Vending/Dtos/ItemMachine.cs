namespace VendingOnlineStore.Clients.Vending.Dtos;

public class ItemMachine
{
    public ItemMachine(string id, string address, double latitude, double longitude, decimal itemPrice)
    {
        Id = id;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        ItemPrice = itemPrice;
    }

    public string Id { get; }

    public string Address { get; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public decimal ItemPrice { get; }
}
