namespace VendingOnlineStore.Models.Catalog;

public class CatalogViewModel
{
    public CatalogViewModel(LocationViewModel location, IEnumerable<OptionViewModel> options)
    {
        Location = location;
        Options = options;
    }

    public LocationViewModel Location { get; }
    public IEnumerable<OptionViewModel> Options { get; }
}