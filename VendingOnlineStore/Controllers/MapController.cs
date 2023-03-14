using Core.Services.Map;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Contracts.Map;

namespace VendingOnlineStore.Controllers;

public class MapController : Controller
{
    private readonly IMapService _mapService;

    public MapController(IMapService mapService)
    {
        _mapService = mapService;
    }

    [HttpGet]
    public async Task<IActionResult> PickupPointsAsync()
    {
        var pickupPoints = await _mapService.GetPickupPointsAsync();
        var result = new FeatureCollection
        {
            Features = pickupPoints.Select(MapToFeatureContract)
        };
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> DefaultLocationAsync()
    {
        var location = await _mapService.GetDefaultLocationAsync();
        var result = new[]
        {
            location.Latitude,
            location.Longitude
        };
        return Json(result);
    }

    private static Feature MapToFeatureContract(PickupPoint pickupPoint, int index)
    {
        var feature = new Feature
        {
            Id = index,
            Geometry = MapToPointContract(pickupPoint),
            Properties = MapToPropertiesContract(pickupPoint)
        };
        return feature;
    }

    private static Point MapToPointContract(PickupPoint pickupPoint)
    {
        var pickupPointCoordinates = pickupPoint.Coordinates;
        var point = new Point
        {
            Coordinates = new[]
            {
                pickupPointCoordinates.Latitude,
                pickupPointCoordinates.Longitude
            }
        };
        return point;
    }

    private static Properties MapToPropertiesContract(PickupPoint pickupPoint)
    {
        var properties = new Properties
        {
            BalloonContentHeader = pickupPoint.Address,
            BalloonContentBody = pickupPoint.Description,
            ClusterCaption = pickupPoint.Address,
            HintContent = pickupPoint.Address
        };
        return properties;
    }
}