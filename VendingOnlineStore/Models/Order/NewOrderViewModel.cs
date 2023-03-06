namespace VendingOnlineStore.Models.Order;

public class NewOrderViewModel
{
    public Guid BagSectionId { get; set; }
    public IReadOnlyCollection<NewOrderContentViewModel> Contents { get; set; } = null!;
}