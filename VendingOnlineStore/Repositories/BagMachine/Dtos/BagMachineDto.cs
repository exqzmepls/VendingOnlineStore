namespace VendingOnlineStore.Repositories.BagMachine.Dtos;

public class BagMachineDto
{
    public BagMachineDto(Guid id, string externalId, IEnumerable<BagItemDto> items)
    {
        Id = id;
        ExternalId = externalId;
        Items = items;
    }

    public Guid Id { get; }
    public string ExternalId { get; }
    public IEnumerable<BagItemDto> Items { get; }
}
