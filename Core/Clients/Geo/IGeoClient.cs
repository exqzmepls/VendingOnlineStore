﻿namespace Core.Clients.Geo;

public interface IGeoClient
{
    public double? GetDistance(double latitude, double longitude);
}
