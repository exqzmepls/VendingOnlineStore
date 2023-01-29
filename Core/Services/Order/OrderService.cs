using Core.Clients.Booking;
using Core.Clients.Payment;
using Core.Extensions;
using Core.Repositories.BagSection;
using Core.Repositories.Order;
using OrderDetailsData = Core.Repositories.Order.OrderDetails;
using OrderBriefData = Core.Repositories.Order.OrderBrief;
using NewOrderData = Core.Repositories.Order.NewOrder;

namespace Core.Services.Order;

public class OrderService : IOrderService
{
    private readonly IBagSectionRepository _bagSectionRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IBookingClient _bookingClient;
    private readonly IPaymentClient _paymentClient;

    public OrderService(IBagSectionRepository bagSectionRepository, IOrderRepository orderRepository,
        IBookingClient bookingClient, IPaymentClient paymentClient)
    {
        _bagSectionRepository = bagSectionRepository;
        _orderRepository = orderRepository;
        _bookingClient = bookingClient;
        _paymentClient = paymentClient;
    }

    public OrderDetails? GetByIdOrDefault(Guid id)
    {
        var order = _orderRepository.GetByIdOrDefault(id);
        if (order == default)
            return default;

        var orderDetails = MapToOrderDetails(order);
        return orderDetails;
    }

    public IQueryable<OrderBrief> GetAll()
    {
        var result = _orderRepository.GetAll()
            //.OrderByDescending(o => o.BookingId)
            .Select(o => MapToOrderBrief(o));
        return result;
    }

    public async Task<string> CreateAsync(NewOrder newOrder)
    {
        var bagSectionId = newOrder.BagSectionId;
        var bagSectionData = await GetBagSectionDataAsync(bagSectionId);

        var newBookingContents = newOrder.Contents.Select(orderContent =>
        {
            var newBookingContent = GetNewBookingContent(orderContent, bagSectionData.Contents);
            return newBookingContent;
        });
        var newBooking = new NewBooking(bagSectionData.PickupPointId, newBookingContents);

        // request to buy
        var bookingDetails = await _bookingClient.CreateBookingAsync(newBooking);
        var bookingContents = bookingDetails.Contents;

        // validate response

        // if not OK throw exception

        // if OK
        // total sum
        var orderTotalPrice = CalculateOrderTotalSum(bookingContents);

        // create payment
        var payment = await _paymentClient.CreatePaymentAsync(orderTotalPrice);

        // create order
        var paymentInfo = new PaymentInfo(payment.Id, payment.Link);

        var pickupPoint = bookingDetails.PickupPoint;
        var pickupPointInfo = new PickupPointInfo(pickupPoint.Id, pickupPoint.Address, pickupPoint.Description);
        var contentsInfo = bookingContents.Select(c =>
        {
            var item = c.Item;
            var contentInfo = new ContentInfo(item.Id, item.Name, item.Description, item.PhotoLink, c.Count, c.Price);
            return contentInfo;
        });
        var newOrderData =
            new NewOrderData(bookingDetails.Id, paymentInfo, pickupPointInfo, contentsInfo, orderTotalPrice);
        var createdOrderId = _orderRepository.CreateOrder(newOrderData);

        // remove from bag
        await _bagSectionRepository.DeleteAsync(bagSectionId);

        // create result with order info and payment link
        return payment.Link;
    }

    private static OrderDetails MapToOrderDetails(OrderDetailsData orderData)
    {
        var status = MapToOrderStatus(orderData.Status);
        var orderPickupPoint = MapToOrderPickupPoint(orderData.PickupPoint);
        var orderContents = orderData.Contents.Select(MapToOrderContent).ToReadOnlyCollection();
        var orderDetails = new OrderDetails(
            orderData.Id,
            orderData.CreationDateUtc,
            status,
            orderData.Payment.Link,
            orderData.ReleaseCode,
            orderPickupPoint,
            orderContents,
            orderData.TotalPrice
        );
        return orderDetails;
    }

    private static OrderContent MapToOrderContent(ContentInfo contentInfo)
    {
        var orderContent = new OrderContent(
            contentInfo.Name,
            contentInfo.Description,
            contentInfo.PhotoLink,
            contentInfo.Count,
            contentInfo.Price
        );
        return orderContent;
    }

    private static OrderPickupPoint MapToOrderPickupPoint(PickupPointInfo pickupPointInfo)
    {
        var orderPickupPoint = new OrderPickupPoint(
            pickupPointInfo.Address,
            pickupPointInfo.Description
        );
        return orderPickupPoint;
    }

    private static OrderBrief MapToOrderBrief(OrderBriefData orderData)
    {
        var status = MapToOrderStatus(orderData.Status);
        var orderBrief = new OrderBrief(orderData.Id, orderData.CreationDateUtc, status);
        return orderBrief;
    }

    private static NewBookingContent GetNewBookingContent(NewOrderContent newOrderContent,
        IEnumerable<BagContentBrief> bagSectionContents)
    {
        var bagContent = bagSectionContents.Single(c => c.Id == newOrderContent.BagContentId);
        var newBookingContent = new NewBookingContent(bagContent.ItemId, newOrderContent.Count);
        return newBookingContent;
    }

    private static decimal CalculateOrderTotalSum(IEnumerable<BookingContent> contents)
    {
        var totalSum = contents.Sum(c => c.Count * c.Price);
        return totalSum;
    }

    private static OrderStatus MapToOrderStatus(Status status)
    {
        return status switch
        {
            Status.WaitingPayment => OrderStatus.WaitingPayment,
            Status.WaitingReceiving => OrderStatus.WaitingReceiving,
            Status.PaymentOverdue => OrderStatus.PaymentOverdue,
            Status.Received => OrderStatus.Received,
            Status.ReceivingOverdue => OrderStatus.ReceivingOverdue,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    private async Task<BagSectionDetails> GetBagSectionDataAsync(Guid bagSectionId)
    {
        var bagSection = await _bagSectionRepository.GetByIdOrDefaultAsync(bagSectionId);
        if (bagSection == default)
            throw new BagSectionNotFoundException("No bag section");
        return bagSection;
    }
}