namespace VendingOnlineStore.Repositories.BagItem.Dtos;

public class NewBagItemDto
{
    public NewBagItemDto(string externalId, Guid machineId)
    {
        ExternalId = externalId;
        MachineId = machineId;
    }

    public string ExternalId { get; }
    public Guid MachineId { get; }
}
