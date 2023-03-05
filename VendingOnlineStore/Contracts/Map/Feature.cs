namespace VendingOnlineStore.Contracts.Map;

public class Feature
{
    public string Type => nameof(Feature);

    public required int Id { get; init; }

    public required Point Geometry { get; init; }

    public required Properties Properties { get; init; }
}