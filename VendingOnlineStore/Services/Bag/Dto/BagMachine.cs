namespace VendingOnlineStore.Services.Bag.Dto;

public class BagMachine
{
    public BagMachine(string id, string address, IEnumerable<BagItem> machineItems)
    {
        Id = id;
        Address = address;
        MachineItems = machineItems;
    }

    public string Id { get; }
    public string Address { get; }
    public IEnumerable<BagItem> MachineItems { get; }
}
