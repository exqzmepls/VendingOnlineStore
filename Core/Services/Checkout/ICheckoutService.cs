namespace Core.Services.Checkout;

public interface ICheckoutService
{
    public Task<CheckoutDetails?> GetCheckoutOrDefaultAsync(Guid bagSectionId);
}

public record CheckoutDetails(Guid BagSectionId, PickupPoint PickupPoint, IReadOnlyCollection<ContentDetails> Contents,
    decimal? TotalPrice);

public record PickupPoint(string Address, string Description);

public record ContentDetails(Guid BagContentId, Item Item, int WantedCount, int SuggestedCount, decimal? TotalPrice);

public record Item(string Name, string PhotoLink, int AvailableCount, decimal? Price);