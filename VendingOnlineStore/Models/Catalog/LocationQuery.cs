using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models.Catalog;

public class LocationQuery
{
    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public double Radius { get; set; }
}