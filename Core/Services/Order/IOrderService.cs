﻿namespace Core.Services.Order;

public interface IOrderService
{
    public IQueryable<OrderBrief> GetAll();

    public Task<string> CreateAsync(NewOrder newOrder);

    public OrderDetails? GetByIdOrDefault(Guid id);
}

public record OrderBrief(Guid Id, DateTime CreationDateUtc, string Status);

public record OrderDetails(Guid Id, DateTime CreationDateUtc, string Status, string PaymentLink,
    OrderPickupPoint PickupPoint, IReadOnlyCollection<OrderContent> Contents, decimal TotalPrice);

public record OrderPickupPoint(string Address, string Description);

public record OrderContent(string Name, string Description, string PhotoLink, int Count, decimal Price);

public record NewOrder(Guid BagSectionId, IReadOnlyCollection<NewOrderContent> Contents);

public record NewOrderContent(Guid BagContentId, int Count);