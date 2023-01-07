using System.Collections.Immutable;
using Core.Services.Bag.Dtos;

namespace Core.Services.Bag;

public class BagService : IBagService
{
    private readonly IBagMachineRepository _bagMachineRepository;
    private readonly IBagItemRepository _bagItemRepository;
    private readonly IVendingClient _vendingClient;

    public BagService(IBagMachineRepository bagMachineRepository, IBagItemRepository bagItemRepository, IVendingClient vendingClient)
    {
        _bagMachineRepository = bagMachineRepository;
        _bagItemRepository = bagItemRepository;
        _vendingClient = vendingClient;
    }

    public async Task<bool> DecreaseItemCountAsync(Guid itemId)
    {
        var item = await _bagItemRepository.GetOrDefaultAsync(itemId);
        if (item == default)
        {
            return false;
        }

        var updatedCount = item.Count - 1;
        if (updatedCount == 0)
        {
            var isRemoved = await RemoveItemAsync(item.Id);
            return isRemoved;
        }

        var updatedItem = new UpdatedBagItemDto(updatedCount);
        var currentItem = await _bagItemRepository.UpdateAsync(item.Id, updatedItem);
        if (currentItem == default)
        {
            return false;
        }

        return true;
    }

    public async Task<IEnumerable<BagMachine>> GetContentAsync()
    {
        var contentSource = _bagMachineRepository.GetAll();
        var content = GetContent(contentSource).ToImmutableArray();

        var isEmpty = !content.Any();
        if (isEmpty)
        {
            return Enumerable.Empty<BagMachine>();
        }

        var machinesItemsQuery = BuildQueryObject(content);
        var machinesItemsInfo = await _vendingClient.GetMachinesItemsInfoAsync(machinesItemsQuery);

        var bagMachines = content.Select(machine =>
        {
            var machineItems = machinesItemsInfo.Single(m => m.MachineInfo.ExternalId == machine.ExternalId);
            var machineInfo = Map(machineItems.MachineInfo);

            var bagMachineItems = machine.Items.Select(item =>
            {
                var machineItem = machineItems.MachineItems.Single(i => i.ItemInfo.ExternalId == item.ExternalId);

                var itemInfo = Map(machineItem.ItemInfo);

                var machineItemPrice = machineItem.Price;
                var machineItemInfo = new MachineItemInfo(machineItem.AvailableCount, machineItemPrice);

                var itemCount = item.Count;
                var itemTotalPrice = itemCount * machineItemPrice;

                var bagItem = new BagItem(item.Id, itemInfo, machineItemInfo, itemCount, itemTotalPrice);
                return bagItem;
            })
            .OrderBy(i => i.Id)
            .ToArray();

            var machineTotalPrice = bagMachineItems.Sum(i => i.TotalPrice);

            var bagMachine = new BagMachine(machine.Id, machineInfo, Array.AsReadOnly(bagMachineItems), machineTotalPrice);
            return bagMachine;
        })
        .OrderBy(m => m.Id);
        return bagMachines;
    }

    public async Task<bool> IncreaseItemCountAsync(Guid itemId)
    {
        var item = await _bagItemRepository.GetOrDefaultAsync(itemId);
        if (item == default)
        {
            return false;
        }

        var updatedCount = item.Count + 1;
        var updatedItem = new UpdatedBagItemDto(updatedCount);
        var currentItem = await _bagItemRepository.UpdateAsync(item.Id, updatedItem);
        if (currentItem == default)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RemoveItemAsync(Guid itemId)
    {
        var item = await _bagItemRepository.GetOrDefaultAsync(itemId);
        if (item == default)
        {
            return false;
        }

        var itemMachineId = item.MachineId;
        var machine = await _bagMachineRepository.GetOrDefaultAsync(itemMachineId);
        if (machine == default)
        {
            return false;
        }

        var machineItemsCount = machine.Items.Count();
        if (machineItemsCount == 1)
        {
            await _bagMachineRepository.DeleteAsync(itemMachineId);
            return true;
        }

        var isRemoved = await _bagItemRepository.DeleteAsync(item.Id);
        return isRemoved;
    }

    private static IEnumerable<BagMachineContent> GetContent(IQueryable<BagMachineDto> contentSource)
    {
        var content = contentSource.ToImmutableArray(); // read and save content
        var result = content.Select(bagMachine =>
        {
            var items = bagMachine.Items.Select(bagItem =>
            {
                var bagItemContent = new BagItemContent(bagItem.Id, bagItem.ExternalId, bagItem.Count);
                return bagItemContent;
            });
            var bagMachineContent = new BagMachineContent(bagMachine.Id, bagMachine.ExternalId, items);
            return bagMachineContent;
        });
        return result;
    }

    private static IEnumerable<MachineItemsQuery> BuildQueryObject(IEnumerable<BagMachineContent> content)
    {
        var queryObject = content.Select(machine =>
        {
            var itemsIds = machine.Items.Select(i => i.ExternalId);
            var machineItemsQuery = new MachineItemsQuery(machine.ExternalId, itemsIds);
            return machineItemsQuery;
        });
        return queryObject;
    }

    private static MachineInfo Map(Clients.Vending.Dtos.MachineInfo machineInfo)
    {
        var result = new MachineInfo(machineInfo.Description, machineInfo.Address);
        return result;
    }

    private static ItemInfo Map(Clients.Vending.Dtos.ItemInfo itemInfo)
    {
        var result = new ItemInfo(itemInfo.Name, itemInfo.Description, itemInfo.PhotoLink);
        return result;
    }
}

public record BagMachineContent
{
    public BagMachineContent(Guid id, string externalId, IEnumerable<BagItemContent> items)
    {
        Id = id;
        ExternalId = externalId;
        Items = Array.AsReadOnly(items.ToArray());
    }

    public Guid Id { get; }
    public string ExternalId { get; }
    public IReadOnlyCollection<BagItemContent> Items { get; }
}

public record BagItemContent
{
    public BagItemContent(Guid id, string externalId, int count)
    {
        Id = id;
        ExternalId = externalId;
        Count = count;
    }

    public Guid Id { get; }
    public string ExternalId { get; }
    public int Count { get; }
}
