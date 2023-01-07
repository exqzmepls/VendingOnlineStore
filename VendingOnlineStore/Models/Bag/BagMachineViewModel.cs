namespace VendingOnlineStore.Models.Bag;

public class BagMachineViewModel
{
	public BagMachineViewModel(Guid id, string description, string address, IEnumerable<BagItemViewModel> bagItems, decimal? totalPrice)
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
	public IEnumerable<BagItemViewModel> BagItems { get; }
	public decimal? TotalPrice { get; }
}
