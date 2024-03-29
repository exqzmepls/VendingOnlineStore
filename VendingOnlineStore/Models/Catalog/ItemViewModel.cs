﻿namespace VendingOnlineStore.Models.Catalog;

public class ItemViewModel
{
    public ItemViewModel(string name, string description, string photoLink)
    {
        Name = name;
        Description = description;
        PhotoLink = photoLink;
    }

    public string Name { get; }
    public string Description { get; }
    public string PhotoLink { get; }
}