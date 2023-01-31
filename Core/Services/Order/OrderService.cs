using Core.Clients.Booking;
using Core.Clients.Payment;
using Core.Extensions;
using Core.Repositories.BagSection;
using Core.Repositories.Order;

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
            .OrderByDescending(o => o.CreationDateUtc)
            .MapToOrderBrief();
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
        var orderPayment = new OrderPaymentData
        {
            Id = payment.Id,
            Link = payment.Link
        };

        var pickupPoint = bookingDetails.PickupPoint;
        var orderPickupPoint = new OrderPickupPointData
        {
            Id = pickupPoint.Id,
            Address = pickupPoint.Address,
            Description = pickupPoint.Description
        };
        var orderContents = bookingContents.Select(c =>
        {
            var item = c.Item;
            var contentInfo = new OrderContentData
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                PhotoLink = item.PhotoLink,
                Count = c.Count,
                Price = c.Price
            };
            return contentInfo;
        });
        var newOrderData = new NewOrderData
        {
            BookingId = bookingDetails.Id,
            Payment = orderPayment,
            PickupPoint = orderPickupPoint,
            Contents = orderContents,
            TotalPrice = orderTotalPrice
        };
        var createdOrderId = _orderRepository.CreateOrder(newOrderData);

        // remove from bag
        await _bagSectionRepository.DeleteAsync(bagSectionId);

        // create result with order info and payment link
        return payment.Link;
    }

    private static OrderDetails MapToOrderDetails(OrderDetailsData orderData)
    {
        var status = orderData.Status.MapToOrderStatus();
        var orderPickupPoint = MapToOrderPickupPoint(orderData.PickupPoint);
        var orderContents = orderData.Contents.Select(MapToOrderContent).ToReadOnlyCollection();
        var orderDetails = new OrderDetails
        {
            Id = orderData.Id,
            CreationDateUtc = orderData.CreationDateUtc,
            Status = status,
            PaymentLink = orderData.Payment.Link,
            ReleaseCode = orderData.ReleaseCode,
            PickupPoint = orderPickupPoint,
            Contents = orderContents,
            TotalPrice = orderData.TotalPrice
        };
        return orderDetails;
    }

    private static OrderContent MapToOrderContent(OrderContentData orderContentData)
    {
        var orderContent = new OrderContent
        {
            Name = orderContentData.Name,
            Description = orderContentData.Description,
            PhotoLink = orderContentData.PhotoLink,
            Count = orderContentData.Count,
            Price = orderContentData.Price
        };
        return orderContent;
    }

    private static OrderPickupPoint MapToOrderPickupPoint(OrderPickupPointData orderPickupPointData)
    {
        var orderPickupPoint = new OrderPickupPoint
        {
            Address = orderPickupPointData.Address,
            Description = orderPickupPointData.Description
        };
        return orderPickupPoint;
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

    private async Task<BagSectionDetails> GetBagSectionDataAsync(Guid bagSectionId)
    {
        var bagSection = await _bagSectionRepository.GetByIdOrDefaultAsync(bagSectionId);
        if (bagSection == default)
            throw new BagSectionNotFoundException("No bag section");
        return bagSection;
    }
}