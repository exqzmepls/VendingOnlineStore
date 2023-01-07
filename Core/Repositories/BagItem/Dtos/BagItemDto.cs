namespace Core.Repositories.BagItem.Dtos;

public class BagItemDto
{
    public BagItemDto(Guid id, Guid machineId, string externalId, int count)
    {
        Id = id;
        MachineId = machineId;
        ExternalId = externalId;
        Count = count;
    }

    public Guid Id { get; }
    public Guid MachineId { get; }
    public string ExternalId { get; }
    public int Count { get; }
}
