using System.ComponentModel;

namespace VendingOnlineStore.Models;

public class ItemViewModel
{
    public ItemViewModel(string id, string name, string description, string photoLink, string priceTag)
    {
        Id = id;
        Name = name;
        Description = description;
        PhotoLink = photoLink;
        PriceTag = priceTag;
    }

    public string Id { get; }

    public string Name { get; }

    public string Description { get; }

    [DisplayName("Photo")]
    public string PhotoLink { get; }

    public string PriceTag { get; }
}
