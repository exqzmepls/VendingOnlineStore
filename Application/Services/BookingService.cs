using Core.Clients.Payment;
using Core.Repositories.Order;
using Core.Services.Booking;
using Core.Services.Booking.Exceptions;

namespace Application.Services;

internal class BookingService : IBookingService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentClient _paymentClient;

    public BookingService(IOrderRepository orderRepository, IPaymentClient paymentClient)
    {
        _orderRepository = orderRepository;
        _paymentClient = paymentClient;
    }

    public async Task OnReleaseAsync(string bookingId)
    {
        var order = GetOrderByBookingId(bookingId);

        await _paymentClient.CapturePaymentAsync(order.PaymentId);

        UpdateOrderStatus(order.Id, OrderStatusData.Received);
    }

    public async Task OnOverdueAsync(string bookingId)
    {
        var order = GetOrderByBookingId(bookingId);

        await _paymentClient.CancelPaymentAsync(order.PaymentId);

        UpdateOrderStatus(order.Id, OrderStatusData.ReceivingOverdue);
    }

    private OrderBriefData GetOrderByBookingId(string bookingId)
    {
        var ordersQueryable = _orderRepository.GetAll();
        var bookingOrder = ordersQueryable.SingleOrDefault(o => o.BookingId == bookingId);
        if (bookingOrder == default)
            throw new OrderNotFoundException();

        return bookingOrder;
    }

    private void UpdateOrderStatus(Guid orderId, OrderStatusData newStatus)
    {
        var orderUpdate = new OrderUpdateData
        {
            NewStatus = newStatus,
            ReleaseCode = default
        };
        try
        {
            _orderRepository.Update(orderId, orderUpdate);
        }
        catch (Exception exception)
        {
            throw new OrderNotUpdatedException("", exception);
        }
    }
}