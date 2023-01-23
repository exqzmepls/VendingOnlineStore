using Core.Extensions;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using OrderEntity = Infrastructure.Models.Order;

namespace Core.Repositories.Order;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _appDbContext;

    public OrderRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IQueryable<OrderBrief> GetAll()
    {
        var result = _appDbContext.Orders.Include(o => o.Payment).Select(o => MapToOrderBrief(o));
        return result;
    }

    public OrderDetails? GetByIdOrDefault(Guid id)
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

    public Guid CreateOrder(NewOrder newOrder)
    {
        var newOrderEntity = MapToOrderEntity(newOrder);
        var entry = _appDbContext.Orders.Add(newOrderEntity);
        _appDbContext.SaveChanges();

        return entry.Entity.Id;
    }

    public void Update(Guid id, OrderUpdate orderUpdate)
    {
        throw new NotImplementedException();
    }

    private static OrderBrief MapToOrderBrief(OrderEntity order)
    {
        var status = MapToStatus(order.Status);
        var orderBrief = new OrderBrief(
            order.Id,
            order.CreationDateUtc,
            status,
            order.Payment!.ExternalId,
            order.BookingId
        );
        return orderBrief;
    }

    private static OrderEntity MapToOrderEntity(NewOrder newOrder)
    {
        var payment = MapToPaymentEntity(newOrder.Payment);
        var pickupPoint = MapToPickupPointEntity(newOrder.PickupPoint);
        var contents = newOrder.Contents.Select(MapToContentEntity).ToReadOnlyCollection();
        var orderEntity = new OrderEntity
        {
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

    private static Payment MapToPaymentEntity(PaymentInfo paymentInfo)
    {
        var paymentEntity = new Payment
        {
            ExternalId = paymentInfo.Id,
            Link = paymentInfo.Link
        };
        return paymentEntity;
    }

    private static OrderPickupPoint MapToPickupPointEntity(PickupPointInfo pickupPointInfo)
    {
        var pickupPointEntity = new OrderPickupPoint
        {
            ExternalId = pickupPointInfo.Id,
            Address = pickupPointInfo.Address,
            Description = pickupPointInfo.Description
        };
        return pickupPointEntity;
    }

    private static OrderContent MapToContentEntity(ContentInfo contentInfo)
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

    private static OrderDetails MapToOrderDetails(OrderEntity order)
    {
        var status = MapToStatus(order.Status);
        var payment = MatToPaymentInfo(order.Payment!);
        var pickupPoint = MapToPickupPointInfo(order.OrderPickupPoint!);
        var contents = order.OrderContents!.Select(MapToContentInfo).ToReadOnlyCollection();
        var orderDetails = new OrderDetails(
            order.Id,
            order.CreationDateUtc,
            order.BookingId,
            status,
            payment,
            pickupPoint,
            contents,
            order.TotalPrice
        );
        return orderDetails;
    }

    private static PaymentInfo MatToPaymentInfo(Payment payment)
    {
        var paymentInfo = new PaymentInfo(
            payment.ExternalId,
            payment.Link
        );
        return paymentInfo;
    }

    private static PickupPointInfo MapToPickupPointInfo(OrderPickupPoint orderPickupPoint)
    {
        var pickupPointInfo = new PickupPointInfo(
            orderPickupPoint.ExternalId,
            orderPickupPoint.Address,
            orderPickupPoint.Description
        );
        return pickupPointInfo;
    }

    private static ContentInfo MapToContentInfo(OrderContent orderContent)
    {
        var contentInfo = new ContentInfo(
            orderContent.ExternalId,
            orderContent.Name,
            orderContent.Description,
            orderContent.PhotoLink,
            orderContent.Count,
            orderContent.Price
        );
        return contentInfo;
    }

    private static Status MapToStatus(OrderStatus orderStatus)
    {
        return orderStatus switch
        {
            OrderStatus.WaitingPayment => Status.WaitingPayment,
            OrderStatus.WaitingReceiving => Status.WaitingReceiving,
            OrderStatus.PaymentOverdue => Status.PaymentOverdue,
            OrderStatus.Received => Status.Received,
            OrderStatus.ReceivingOverdue => Status.ReceivingOverdue,
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatus), orderStatus, null)
        };
    }
}