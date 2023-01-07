namespace Core.Services.Bag.Dtos;

public class BagMachine
{
    public BagMachine(Guid id, MachineInfo machineInfo, IReadOnlyCollection<BagItem> machineItems, decimal? totalPrice)
    {
        Id = id;
        MachineInfo = machineInfo;
        MachineItems = machineItems;
        TotalPrice = totalPrice;
    }

    public Guid Id { get; }
    public MachineInfo MachineInfo { get; }
    public IReadOnlyCollection<BagItem> MachineItems { get; }
    public decimal? TotalPrice { get; }
}
