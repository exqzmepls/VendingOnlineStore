namespace Core.Services.Catalog;

public interface ICatalogService
{
    public IReadOnlyCollection<OptionDetails> GetOptions(Location location);

    public Task<BagContent> AddToBagAsync(NewBagContent newBagContent);

    public Task<BagContent> IncreaseContentCountAsync(Guid bagContentId);

    public Task<BagContent> DecreaseContentCountAsync(Guid bagContentId);
}

public record Location(double Latitude, double Longitude, double Radius);

public record OptionDetails(ItemDetails Item, IReadOnlyCollection<ContentDetails> Contents);

public record ItemDetails(string Name, string Description, string PhotoLink);

public record ContentDetails(PickupPointDetails PickupPoint, decimal Price, BagContent BagContent);

public record PickupPointDetails(string Address, string Description);

public record BagContent(string ItemId, string PickupPointId, BagEntrance? BagEntrance);

public record BagEntrance(Guid BagContentId, int Count);

public record NewBagContent(string ItemId, string PickupPointId);