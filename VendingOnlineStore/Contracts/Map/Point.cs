namespace VendingOnlineStore.Contracts.Map;

public class Point
{
    public string Type => nameof(Point);

    public required IEnumerable<double> Coordinates { get; init; }
}