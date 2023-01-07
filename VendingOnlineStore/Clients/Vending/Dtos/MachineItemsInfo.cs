namespace VendingOnlineStore.Clients.Vending.Dtos;

public class MachineItemsInfo
{
    public MachineItemsInfo(MachineInfo machineInfo, IEnumerable<MachineItemInfo> machineItems)
    {
        MachineInfo = machineInfo;
        MachineItems = machineItems;
    }

    public MachineInfo MachineInfo { get; }
    public IEnumerable<MachineItemInfo> MachineItems { get; }
}