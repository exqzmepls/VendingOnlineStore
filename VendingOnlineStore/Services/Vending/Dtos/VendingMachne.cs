namespace VendingOnlineStore.Services.Vending.Dtos;

public class VendingMachine
{
    public VendingMachine(string id, string description, string address, string distance)
    {
        Id = id;
        Description = description;
        Address = address;
        Distance = distance;
    }

    public string Id { get; }

    public string Description { get; }

    public string Address { get; }

    public string Distance { get; }
}
