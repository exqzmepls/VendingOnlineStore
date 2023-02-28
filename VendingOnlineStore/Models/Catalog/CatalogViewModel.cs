namespace VendingOnlineStore.Models.Catalog;

public class CatalogViewModel
{
    public CatalogViewModel(MapViewModel map, IEnumerable<OptionViewModel> options)
    {
        Map = map;
        Options = options;
    }

    public MapViewModel Map { get; }
    public IEnumerable<OptionViewModel> Options { get; }
}