using Core.Clients.Booking;
using Core.Repositories.Order;
using Core.Services.Payment.Exceptions;
using Microsoft.Extensions.Logging;

namespace Core.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly IBookingClient _bookingClient;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IBookingClient bookingClient, IOrderRepository orderRepository,
        ILogger<PaymentService> logger)
    {
        _bookingClient = bookingClient;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task ProcessEventAsync(string eventName, string paymentId)
    {
        switch (eventName)
        {
            case "payment.succeeded":
                OnSucceeded(paymentId);
                break;
            case "payment.canceled":
                await OnCanceledAsync(paymentId);
                break;
            case "payment.waiting_for_capture":
                await OnWaitingForCaptureAsync(paymentId);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventName));
        }
    }

    private void OnSucceeded(string paymentId)
    {
        _logger.LogInformation("Payment {PaymentId} succeeded", paymentId);
    }

    private async Task OnCanceledAsync(string paymentId)
    {
        var order = GetOrderByPaymentId(paymentId);

        await DropOrderBookingAsync(order);

        MakeOrderCanceled(order);
    }

    private async Task OnWaitingForCaptureAsync(string paymentId)
    {
        var order = GetOrderByPaymentId(paymentId);

        var releaseCode = await ApproveOrderBookingAsync(order);

        MakeOrderReleaseReady(order, releaseCode);
    }

    private OrderBriefData GetOrderByPaymentId(string paymentId)
    {
        var order = _orderRepository.GetAll().SingleOrDefault(o => o.PaymentId == paymentId);
        return order ?? throw new OrderNotFoundException($"Order with Payment Id = {paymentId}");
    }

    private async Task DropOrderBookingAsync(OrderBriefData order)
    {
        var bookingId = order.BookingId;
        try
        {
            await _bookingClient.DropBookingAsync(bookingId);
        }
        catch (Exception exception)
        {
            throw new BookingNotDropException($"Booking Id = {bookingId}", exception);
        }
    }

    private void MakeOrderCanceled(OrderBriefData order)
    {
        var orderUpdate = new OrderUpdateData
        {
            NewStatus = OrderStatusData.PaymentOverdue,
            ReleaseCode = default
        };
        UpdateOrder(order, orderUpdate);
    }

    private async Task<int> ApproveOrderBookingAsync(OrderBriefData order)
    {
        var bookingId = order.BookingId;
        try
        {
            var releaseCode = await _bookingClient.ApproveBookingAsync(bookingId);
            return releaseCode;
        }
        catch (Exception exception)
        {
            throw new BookingNotApprovedException($"Booking Id = {bookingId}", exception);
        }
    }

    private void MakeOrderReleaseReady(OrderBriefData order, int releaseCode)
    {
        var orderUpdate = new OrderUpdateData
        {
            NewStatus = OrderStatusData.WaitingPayment,
            ReleaseCode = releaseCode
        };
        UpdateOrder(order, orderUpdate);
    }

    private void UpdateOrder(OrderBriefData order, OrderUpdateData update)
    {
        var orderId = order.Id;
        try
        {
            _orderRepository.Update(orderId, update);
        }
        catch (Exception exception)
        {
            throw new OrderNotUpdatedException($"Order Id = {orderId}", exception);
        }
    }
}