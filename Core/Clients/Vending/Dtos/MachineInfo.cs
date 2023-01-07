namespace Core.Clients.Vending.Dtos;

public class MachineInfo
{
    public MachineInfo(string externalId, string description, string address)
    {
        ExternalId = externalId;
        Description = description;
        Address = address;
    }

    public string ExternalId { get; }
    public string Description { get; }
    public string Address { get; }
}