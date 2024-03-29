﻿using Core.Clients.Geo;
using Core.Clients.Vending;
using Core.Services.Vending;
using Core.Services.Vending.Dtos;

namespace Application.Services;

internal class VendingService : IVendingService
{
    private readonly IVendingClient _vendingClient;
    private readonly IGeoClient _geoClient;

    public VendingService(IVendingClient vendingClient, IGeoClient geoClient)
    {
        _vendingClient = vendingClient;
        _geoClient = geoClient;
    }

    public async Task<IEnumerable<VendingMachine>> GetMachinesAsync()
    {
        var receivedMachines = await _vendingClient.GetPickupPointsAsync();
        var machines = receivedMachines.Select(m =>
        {
            var distance = _geoClient.GetDistance(m.Latitude, m.Longitude);
            var machine = new VendingMachine(m.Id, m.Description, m.Address, $"{Math.Round(distance ?? 0)} m");
            return machine;
        });
        return machines;
    }

    public async Task<IEnumerable<MachineSlot>> GetMachineSlotsAsync(string machineId)
    {
        var receivedSlots = await _vendingClient.GetPickupPointSlotsAsync(machineId);
        var slots = receivedSlots.Select(s =>
        {
            var item = s.Item;
            var slot = new MachineSlot(s.Id, item.Name, item.Description, item.PhotoLink, s.Price, s.Count);
            return slot;
        });
        return slots;
    }
}
