using Core.Clients.Vending.Dtos;

namespace Core.Clients.Vending;

public class DummyVendingClient : IVendingClient
{
    public async Task<Item> GetItemAsync(string itemId)
    {
        var items = await GetItemsAsync();
        var item = items.Single(i => i.Id == itemId);
        return item;
    }

    public async Task<IEnumerable<ItemMachine>> GetItemMachinesAsync(string itemId)
    {
        await Task.Delay(35);
        var itemPrice = itemId == "i1" ? 10.99m : itemId == "i2" ? 15.90m : 18.50m;
        var machines = new ItemMachine[]
        {
            new ItemMachine("m1", "бульвар Гагарина, 37А, Пермь", 58.016314, 56.276928, itemPrice),
            new ItemMachine("m2", "улица Революции, 13, Пермь", 58.007545, 56.261786, itemPrice + 2),
            new ItemMachine("m3", "Студенческая улица, 38, Пермь", 58.010662, 56.281509, itemPrice + 5)
        };
        return machines;
    }

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        await Task.Delay(35);
        var items = new Item[]
        {
            new Item("i1", "water", "cool water", "https://www.topfreeshop.ru/4515-9179-large/distillirovannaja-voda.jpg", 10.99m),
            new Item("i2", "chocolate", "cool chocolate", "https://alania-market.ru/image/cache/catalog/konditerskie-izdeliya/shokolad/7/61099454-1-640x640.jpg", 15.90m),
            new Item("i3", "cookies", "cool cookies", "https://i.pinimg.com/originals/5d/d5/db/5dd5dbc145317cb6a9fc7fbdfcdcaba5.jpg", 18.50m)
        };
        return items;
    }

    public async Task<IEnumerable<VendingMachine>> GetMachinesAsync()
    {
        await Task.Delay(35);
        var machines = new VendingMachine[]
        {
            new VendingMachine("m1", "hse 3", "бульвар Гагарина, 37А, Пермь", 58.016314, 56.276928),
            new VendingMachine("m2", "shopping mall", "улица Революции, 13, Пермь", 58.007545, 56.261786),
            new VendingMachine("m3", "hse 1", "Студенческая улица, 38, Пермь", 58.010662, 56.281509)
        };
        return machines;
    }

    public async Task<IEnumerable<MachineItemsInfo>> GetMachinesItemsInfoAsync(IEnumerable<MachineItemsQuery> machineItems)
    {
        var machines = (await GetMachinesAsync()).ToArray();
        var result = machineItems.Select(m =>
        {
            var machine = machines.Single(x => x.Id == m.MachineId);

            var machineInfo = new MachineInfo(machine.Id, machine.Description, machine.Address);

            var itemsInfo = m.ItemsIds.Select(i =>
            {
                var item = GetItemAsync(i).Result;

                var itemInfo = new ItemInfo(item.Id, item.Name, item.Description, item.PhotoLink);

                var slots = GetMachineSlotsAsync(machine.Id).Result;
                var itemSlot = slots.SingleOrDefault(s => s.Item.ExternalId == item.Id);
                var count = itemSlot?.Count ?? 0;
                var price = itemSlot?.Price;

                var machineItemInfo = new MachineItemInfo(itemInfo, count, price);
                return machineItemInfo;
            });

            var machineItemsInfo = new MachineItemsInfo(machineInfo, itemsInfo);
            return machineItemsInfo;
        });
        return result;
    }

    public async Task<IEnumerable<Slot>> GetMachineSlotsAsync(string machineId)
    {
        await Task.Delay(35);
        var slots = new Slot[]
        {
            new Slot("s1", new ItemInfo("i1", "water", "cool water", "https://www.topfreeshop.ru/4515-9179-large/distillirovannaja-voda.jpg"), 10.99m, 12),
            new Slot("s2", new ItemInfo("i2", "chocolate", "cool chocolate", "https://alania-market.ru/image/cache/catalog/konditerskie-izdeliya/shokolad/7/61099454-1-640x640.jpg"), 15.90m, 20),
            new Slot("s3", new ItemInfo("i3", "cookies", "cool cookies", "https://i.pinimg.com/originals/5d/d5/db/5dd5dbc145317cb6a9fc7fbdfcdcaba5.jpg"), 18.50m, 9)
        };
        return slots;
    }
}
