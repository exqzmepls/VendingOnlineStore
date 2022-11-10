using System.ComponentModel.DataAnnotations;

namespace VendingOnlineStore.Models;

public class SlotViewModel
{
    public SlotViewModel(string id, string itemName, string itemDescription, string itemPhotoLink, decimal price, int count)
    {
        Id = id;
        ItemName = itemName;
        ItemDescription = itemDescription;
        ItemPhotoLink = itemPhotoLink;
        Price = price;
        Count = count;
    }

    public string Id { get; }

    public string ItemName { get; }

    public string ItemDescription { get; }

    [Display(Name = "Photo")]
    public string ItemPhotoLink { get; }

    public decimal Price { get; }

    public int Count { get; }
}
