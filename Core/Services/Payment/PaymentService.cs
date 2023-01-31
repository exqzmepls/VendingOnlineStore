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

    private OrderBrief GetOrderByPaymentId(string paymentId)
    {
        var order = _orderRepository.GetAll().ToList().SingleOrDefault(o => o.PaymentId == paymentId);
        return order ?? throw new OrderNotFoundException($"Order with Payment Id = {paymentId}");
    }

    private async Task DropOrderBookingAsync(OrderBrief order)
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

    private void MakeOrderCanceled(OrderBrief order)
    {
        var orderUpdate = new OrderUpdate(Status.PaymentOverdue, default);
        UpdateOrder(order, orderUpdate);
    }

    private async Task<int> ApproveOrderBookingAsync(OrderBrief order)
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

    private void MakeOrderReleaseReady(OrderBrief order, int releaseCode)
    {
        var orderUpdate = new OrderUpdate(Status.WaitingReceiving, releaseCode);
        UpdateOrder(order, orderUpdate);
    }

    private void UpdateOrder(OrderBrief order, OrderUpdate update)
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