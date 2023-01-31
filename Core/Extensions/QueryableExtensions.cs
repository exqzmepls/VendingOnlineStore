using Core.Repositories.Order;
using Core.Services.Order;
using Infrastructure.Models;

namespace Core.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<OrderBrief> MapToOrderBrief(this IQueryable<OrderBriefData> source)
    {
        return source.Select(orderData => new OrderBrief
        {
            Id = orderData.Id,
            CreationDateUtc = orderData.CreationDateUtc,
            Status = orderData.Status.MapToOrderStatus()
        });
    }

    public static IQueryable<OrderBriefData> MapToOrderBriefData(this IQueryable<Order> source)
    {
        return source.Select(order => new OrderBriefData
        {
            Id = order.Id,
            CreationDateUtc = order.CreationDateUtc,
            Status = order.Status.MapToOrderStatusData(),
            PaymentId = order.Payment!.ExternalId,
            BookingId = order.BookingId
        });
    }
}