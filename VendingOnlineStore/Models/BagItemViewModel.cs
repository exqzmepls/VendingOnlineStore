namespace VendingOnlineStore.Models;

public class BagItemViewModel
{
    public BagItemViewModel(string id, string name, string photoLink, int count)
    {
        Id = id;
        Name = name;
        PhotoLink = photoLink;
        Count = count;
    }

    public string Id { get; }

    public string Name { get; }

    public string PhotoLink { get; }

    public int Count { get; }
}
