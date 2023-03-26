using Application.Extensions;
using Core.Extensions;
using Core.Repositories;
using Core.Repositories.Order;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using OrderEntity = Infrastructure.Models.Order;

namespace Application.Repositories;

internal class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _appDbContext;

    public OrderRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IQueryable<OrderBriefData> GetAll()
    {
        var result = _appDbContext.Orders
            .Include(o => o.Payment)
            .Include(o => o.OrderPickupPoint)
            .MapToOrderBriefData();
        return result;
    }

    public OrderDetailsData? GetByIdOrDefault(Guid id)
    {
        var order = _appDbContext.Orders.Find(id);
        if (order == default)
            return default;

        _appDbContext.Entry(order).Reference(o => o.Payment).Load();
        _appDbContext.Entry(order).Reference(o => o.OrderPickupPoint).Load();
        _appDbContext.Entry(order).Collection(o => o.OrderContents!).Load();
        var result = MapToOrderDetails(order);
        return result;
    }

    public Guid CreateOrder(NewOrderData newOrder)
    {
        var newOrderEntity = MapToOrderEntity(newOrder);
        var entry = _appDbContext.Orders.Add(newOrderEntity);
        _appDbContext.SaveChanges();

        return entry.Entity.Id;
    }

    public void Update(Guid id, OrderUpdateData orderUpdate)
    {
        var order = _appDbContext.Orders.Find(id);
        if (order == default)
            throw new OrderDoesNotExistException();

        if (orderUpdate.NewStatus != default)
            order.Status = MapToOrderStatus(orderUpdate.NewStatus.Value);

        if (orderUpdate.ReleaseCode != default)
            order.ReleaseCode = orderUpdate.ReleaseCode;

        try
        {
            _appDbContext.SaveChanges();
        }
        catch (Exception exception)
        {
            throw new DbException("Order update exception", exception);
        }
    }

    private static OrderEntity MapToOrderEntity(NewOrderData newOrder)
    {
        var payment = MapToPaymentEntity(newOrder.Payment);
        var pickupPoint = MapToPickupPointEntity(newOrder.PickupPoint);
        var contents = newOrder.Contents.Select(MapToContentEntity).ToReadOnlyCollection();
        var orderEntity = new OrderEntity
        {
            UserId = newOrder.UserId,
            BookingId = newOrder.BookingId,
            CreationDateUtc = DateTime.UtcNow,
            Status = OrderStatus.WaitingPayment,
            TotalPrice = newOrder.TotalPrice,
            Payment = payment,
            OrderPickupPoint = pickupPoint,
            OrderContents = contents
        };
        return orderEntity;
    }

    private static Payment MapToPaymentEntity(OrderPaymentData paymentInfo)
    {
        var paymentEntity = new Payment
        {
            ExternalId = paymentInfo.Id,
            Link = paymentInfo.Link
        };
        return paymentEntity;
    }

    private static OrderPickupPoint MapToPickupPointEntity(OrderPickupPointData pickupPointInfo)
    {
        var pickupPointEntity = new OrderPickupPoint
        {
            ExternalId = pickupPointInfo.Id,
            Address = pickupPointInfo.Address,
            Description = pickupPointInfo.Description
        };
        return pickupPointEntity;
    }

    private static OrderContent MapToContentEntity(OrderContentData contentInfo)
    {
        var contentEntity = new OrderContent
        {
            ExternalId = contentInfo.Id,
            Name = contentInfo.Name,
            Description = contentInfo.Description,
            PhotoLink = contentInfo.PhotoLink,
            Count = contentInfo.Count,
            Price = contentInfo.Price
        };
        return contentEntity;
    }

    private static OrderDetailsData MapToOrderDetails(OrderEntity order)
    {
        var status = order.Status.MapToOrderStatusData();
        var payment = MatToPaymentInfo(order.Payment!);
        var pickupPoint = MapToPickupPointInfo(order.OrderPickupPoint!);
        var contents = order.OrderContents!.Select(MapToContentInfo).ToReadOnlyCollection();
        var orderDetails = new OrderDetailsData
        {
            Id = order.Id,
            UserId = order.UserId,
            CreationDateUtc = order.CreationDateUtc,
            Status = status,
            Payment = payment,
            ReleaseCode = order.ReleaseCode,
            PickupPoint = pickupPoint,
            Contents = contents,
            TotalPrice = order.TotalPrice,
        };
        return orderDetails;
    }

    private static OrderPaymentData MatToPaymentInfo(Payment payment)
    {
        var paymentInfo = new OrderPaymentData
        {
            Id = payment.ExternalId,
            Link = payment.Link
        };
        return paymentInfo;
    }

    private static OrderPickupPointData MapToPickupPointInfo(OrderPickupPoint orderPickupPoint)
    {
        var pickupPointInfo = new OrderPickupPointData
        {
            Id = orderPickupPoint.ExternalId,
            Address = orderPickupPoint.Address,
            Description = orderPickupPoint.Description,
        };
        return pickupPointInfo;
    }

    private static OrderContentData MapToContentInfo(OrderContent orderContent)
    {
        var contentInfo = new OrderContentData
        {
            Id = orderContent.ExternalId,
            Name = orderContent.Name,
            Description = orderContent.Description,
            PhotoLink = orderContent.PhotoLink,
            Count = orderContent.Count,
            Price = orderContent.Price
        };
        return contentInfo;
    }

    private static OrderStatus MapToOrderStatus(OrderStatusData status)
    {
        return status switch
        {
            OrderStatusData.WaitingPayment => OrderStatus.WaitingPayment,
            OrderStatusData.WaitingReceiving => OrderStatus.WaitingReceiving,
            OrderStatusData.PaymentOverdue => OrderStatus.PaymentOverdue,
            OrderStatusData.Received => OrderStatus.Received,
            OrderStatusData.ReceivingOverdue => OrderStatus.ReceivingOverdue,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}