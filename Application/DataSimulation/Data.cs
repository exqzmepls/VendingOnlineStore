namespace Application.DataSimulation;

internal static class Data
{
    public static readonly IList<SimulatedPickupPoint> PickupPoints = new List<SimulatedPickupPoint>
    {
        new("p1", "hse 3", "бульвар Гагарина, 37А, Пермь", 58.016314, 56.276928),
        new("p2", "shopping mall", "улица Революции, 13, Пермь", 58.007545, 56.261786),
        new("p3", "hse 1", "Студенческая улица, 38, Пермь", 58.010662, 56.281509)
    };

    public static readonly IList<SimulatedItem> Items = new List<SimulatedItem>
    {
        new("i1", "water", "cool water", "https://localhost:7128/img/water.jpg"),
        new("i2", "chocolate", "cool chocolate", "https://localhost:7128/img/choko.jpg"),
        new("i3", "cookies", "cool cookies", "https://localhost:7128/img/cookies.jpg"),
        new("i4", "cola", "cool cola", "https://localhost:7128/img/cola.jpg"),
        new("i5", "crisps", "cool crisps", "https://localhost:7128/img/crisps.jpg"),
        new("i6", "snickers", "cool snickers", "https://localhost:7128/img/snickers.jpg"),
        new("i7", "suhariki", "cool suhariki", "https://localhost:7128/img/suhari.jpg")
    };

    public static readonly IList<SimulatedPickupPointContent> PickupPointsContents =
        new List<SimulatedPickupPointContent>
        {
            // 1 p
            new("s1", PickupPoints[0], Items[0], 10, 10.99m), // 1
            new("s2", PickupPoints[0], Items[1], 15, 18.99m), // 2
            new("s3", PickupPoints[0], Items[2], 7, 25.99m), // 3
            new("s4", PickupPoints[0], Items[4], 9, 40.99m), // 5
            new("s5", PickupPoints[0], Items[5], 1, 13.99m), // 6
            // 2 p
            new("s6", PickupPoints[1], Items[0], 3, 8.99m), // 1
            new("s7", PickupPoints[1], Items[1], 30, 35.99m), // 2
            new("s8", PickupPoints[1], Items[2], 13, 23.99m), // 3
            new("s9", PickupPoints[1], Items[6], 19, 29.99m), // 7
            // 3 p
            new("s10", PickupPoints[2], Items[0], 28, 17.99m), // 1
            new("s11", PickupPoints[2], Items[1], 24, 19.99m), // 2
            new("s12", PickupPoints[2], Items[2], 14, 22.99m), // 3
            new("s13", PickupPoints[2], Items[3], 5, 21.99m), // 4
            new("s14", PickupPoints[2], Items[4], 4, 13.99m), // 5
            new("s15", PickupPoints[2], Items[5], 16, 32.99m) // 6
        };
}

public record SimulatedPickupPoint(string Id, string Description, string Address, double Latitude, double Longitude);

public record SimulatedItem(string Id, string Name, string Description, string PhotoLink);

public record SimulatedPickupPointContent(string Id, SimulatedPickupPoint PickupPoint, SimulatedItem Item,
    int AvailableCount, decimal Price);