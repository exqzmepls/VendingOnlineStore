namespace VendingOnlineStore.Models.Catalog;

public class ContentCollectionViewModel
{
    public ContentCollectionViewModel(int optionIndex, IEnumerable<ContentViewModel> contents)
    {
        OptionIndex = optionIndex;
        Contents = contents;
    }

    public int OptionIndex { get; }
    public IEnumerable<ContentViewModel> Contents { get; }
}