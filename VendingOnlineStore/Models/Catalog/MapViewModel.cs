namespace VendingOnlineStore.Models.Catalog;

public class MapViewModel
{
    public MapViewModel(double defaultLatitude, double defaultLongitude, double defaultRadius)
    {
        DefaultLatitude = defaultLatitude;
        DefaultLongitude = defaultLongitude;
        DefaultRadius = defaultRadius;
    }

    public double DefaultLatitude { get; }
    public double DefaultLongitude { get; }
    public double DefaultRadius { get; }
}