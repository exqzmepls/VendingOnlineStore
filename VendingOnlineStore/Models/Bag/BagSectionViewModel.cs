namespace VendingOnlineStore.Models.Bag;

public class BagSectionViewModel
{
	public BagSectionViewModel(Guid id, string description, string address, IEnumerable<BagContentViewModel> bagItems, decimal? totalPrice)
	{
		Id = id;
		Description = description;
		Address = address;
		BagItems = bagItems;
		TotalPrice = totalPrice;
	}

	public Guid Id { get; }
	public string Description { get; }
	public string Address { get; }
	public IEnumerable<BagContentViewModel> BagItems { get; }
	public decimal? TotalPrice { get; }
}
