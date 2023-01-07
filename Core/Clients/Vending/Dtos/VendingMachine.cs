namespace Core.Clients.Vending.Dtos;

public class VendingMachine
{
    public VendingMachine(string id, string description, string address, double latitude, double longitude)
    {
        Id = id;
        Description = description;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public string Id { get; }

    public string Description { get; }

    public string Address { get; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
