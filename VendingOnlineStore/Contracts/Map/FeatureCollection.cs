namespace VendingOnlineStore.Contracts.Map;

public class FeatureCollection
{
    public string Type => nameof(FeatureCollection);

    public required IEnumerable<Feature> Features { get; init; }
}