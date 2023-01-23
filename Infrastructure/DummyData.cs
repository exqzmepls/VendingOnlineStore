namespace Infrastructure;

public static class DummyData
{
    public static readonly IList<DummyPickupPoint> PickupPoints = new List<DummyPickupPoint>
    {
        new("m1", "hse 3", "бульвар Гагарина, 37А, Пермь", 58.016314, 56.276928),
        new("m2", "shopping mall", "улица Революции, 13, Пермь", 58.007545, 56.261786),
        new("m3", "hse 1", "Студенческая улица, 38, Пермь", 58.010662, 56.281509)
    };

    public static readonly IList<DummyItem> Items = new List<DummyItem>
    {
        new("i1", "water", "cool water",
            "https://www.topfreeshop.ru/4515-9179-large/distillirovannaja-voda.jpg"),
        new("i2", "chocolate", "cool chocolate",
            "https://alania-market.ru/image/cache/catalog/konditerskie-izdeliya/shokolad/7/61099454-1-640x640.jpg"),
        new("i3", "cookies", "cool cookies",
            "https://i.pinimg.com/originals/5d/d5/db/5dd5dbc145317cb6a9fc7fbdfcdcaba5.jpg")
    };

    public static readonly IList<DummyPickupPointContent> PickupPointsContents = new List<DummyPickupPointContent>
    {
        new("s1", PickupPoints[0], Items[0], 10, 10.99m), // 1
        new("s2", PickupPoints[0], Items[1], 15, 18.99m), // 2
        new("s3", PickupPoints[0], Items[2], 7, 25.99m), // 3
        new("s4", PickupPoints[1], Items[0], 3, 8.99m), // 1
        new("s5", PickupPoints[1], Items[1], 30, 35.99m), // 2
        new("s6", PickupPoints[1], Items[2], 13, 23.99m), // 3
        new("s7", PickupPoints[2], Items[0], 28, 17.99m), // 1
        new("s8", PickupPoints[2], Items[1], 24, 19.99m), // 2
        new("s9", PickupPoints[2], Items[2], 14, 22.99m) // 3
    };
}

public record DummyItem(string Id, string Name, string Description, string PhotoLink);

public record DummyPickupPoint(string Id, string Description, string Address, double Latitude, double Longitude);

public record DummyPickupPointContent(string Id, DummyPickupPoint PickupPoint, DummyItem Item, int AvailableCount,
    decimal Price);