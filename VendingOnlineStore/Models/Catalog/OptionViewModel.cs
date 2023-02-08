namespace VendingOnlineStore.Models.Catalog;

public class OptionViewModel
{
    public OptionViewModel(ItemViewModel item, ContentCollectionViewModel contentCollection)
    {
        Item = item;
        ContentCollection = contentCollection;
    }

    public ItemViewModel Item { get; }
    public ContentCollectionViewModel ContentCollection { get; }
}