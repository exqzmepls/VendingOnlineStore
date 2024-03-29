﻿using Core.Clients.Geo;
using GeoCoordinatePortable;

namespace Application.Clients;

internal class GeoClient : IGeoClient
{
    public double? GetDistance(double latitude, double longitude)
    {
        var position = new GeoCoordinate(58.020495, 56.276174);
        var destination = new GeoCoordinate(latitude, longitude);
        var distance = position?.GetDistanceTo(destination);
        return distance;
    }
}
