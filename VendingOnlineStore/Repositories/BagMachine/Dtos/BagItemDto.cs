namespace VendingOnlineStore.Repositories.BagMachine.Dtos;

public class BagItemDto
{
    public BagItemDto(string id, string externalId, int count)
    {
        Id = id;
        ExternalId = externalId;
        Count = count;
    }

    public string Id { get; }
    public string ExternalId { get; }
    public int Count { get; }
}
