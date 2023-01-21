namespace VendingOnlineStore.Models.Order;

public class OrderPickupPointViewModel
{
    public OrderPickupPointViewModel(string address, string description)
    {
        Address = address;
        Description = description;
    }

    public string Address { get; }
    public string Description { get; }
}