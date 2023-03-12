using Core.Extensions;
using Core.Repositories.BagSection;
using Core.Repositories.Order;
using Core.Services.Order;
using Infrastructure.Models;

namespace Application.Extensions;

internal static class QueryableExtensions
{
    public static IQueryable<OrderBrief> MapToOrderBrief(this IQueryable<OrderBriefData> source)
    {
        return source.Select(orderData => new OrderBrief
        {
            Id = orderData.Id,
            CreationDateUtc = orderData.CreationDateUtc,
            Status = orderData.Status.MapToOrderStatus(),
            PickupPointAddress = orderData.PickupPointAddress,
            TotalPrice = orderData.TotalPrice
        });
    }

    public static IQueryable<OrderBriefData> MapToOrderBriefData(this IQueryable<Order> source)
    {
        return source.Select(order => new OrderBriefData
        {
            Id = order.Id,
            UserId = order.UserId,
            CreationDateUtc = order.CreationDateUtc,
            Status = order.Status.MapToOrderStatusData(),
            PaymentId = order.Payment!.ExternalId,
            BookingId = order.BookingId,
            PickupPointAddress = order.OrderPickupPoint!.Address,
            TotalPrice = order.TotalPrice
        });
    }

    public static IQueryable<BagSectionDetailsData> MapToBagSectionDetailsData(this IQueryable<BagSection> source)
    {
        return source.Select(bagSection => new BagSectionDetailsData
        {
            Id = bagSection.Id,
            UserId = bagSection.UserId,
            PickupPointId = bagSection.PickupPointId,
            Contents = bagSection.BagContents!.Select(bagContent => new BagContentBriefData
            {
                Id = bagContent.Id,
                ItemId = bagContent.ItemId,
                Count = bagContent.Count
            }).ToReadOnlyCollection()
        });
    }
}