namespace VendingOnlineStore.Models.Catalog;

public class PickupPointViewModel
{
    public PickupPointViewModel(string address, string description)
    {
        Address = address;
        Description = description;
    }
    
    public string Address { get; }
    public string Description { get; }
}