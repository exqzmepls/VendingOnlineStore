namespace VendingOnlineStore.Repositories.BagMachine.Dtos;

public class BagMachineDto
{
    public BagMachineDto(string id, string externalId, IEnumerable<BagItemDto> items)
    {
        Id = id;
        ExternalId = externalId;
        Items = items;
    }

    public string Id { get; }
    public string ExternalId { get; }
    public IEnumerable<BagItemDto> Items { get; }
}
