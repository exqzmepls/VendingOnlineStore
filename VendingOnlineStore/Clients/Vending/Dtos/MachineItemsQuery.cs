namespace VendingOnlineStore.Clients.Vending.Dtos;

public class MachineItemsQuery
{
    public MachineItemsQuery(string machineId, IEnumerable<string> itemsIds)
    {
        MachineId = machineId;
        ItemsIds = itemsIds;
    }

    public string MachineId { get; }
    public IEnumerable<string> ItemsIds { get; }
}