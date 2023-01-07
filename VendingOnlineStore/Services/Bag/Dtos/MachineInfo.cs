namespace VendingOnlineStore.Services.Bag.Dtos;

public class MachineInfo
{
    public MachineInfo(string description, string address)
    {
        Description = description;
        Address = address;
    }

    public string Description { get; }
    public string Address { get; }
}