namespace VendingOnlineStore.Models.Catalog;

public class LocationViewModel
{
    public LocationViewModel(double latitude, double longitude, double radius)
    {
        Latitude = latitude;
        Longitude = longitude;
        Radius = radius;
    }

    public double Latitude { get; }
    public double Longitude { get; }
    public double Radius { get; }
}