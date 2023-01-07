namespace Core.Repositories.BagMachine.Dtos;

public class BagItemDto
{
    public BagItemDto(Guid id, string externalId, int count)
    {
        Id = id;
        ExternalId = externalId;
        Count = count;
    }

    public Guid Id { get; }
    public string ExternalId { get; }
    public int Count { get; }
}
