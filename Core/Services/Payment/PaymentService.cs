using Core.Clients.Booking;
using Core.Repositories.Order;
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
            case "1":
                Succeeded(paymentId);
                break;
            case "2":
                await CanceledAsync(paymentId);
                break;
            case "3":
                await WaitingForCaptureAsync(paymentId);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eventName));
        }
    }

    private void Succeeded(string paymentId)
    {
        _logger.LogInformation("Payment {PaymentId} succeeded", paymentId);
    }

    private async Task CanceledAsync(string paymentId)
    {
        var order = GetOrderByPaymentId(paymentId);

        var bookingId = order.BookingId;
        try
        {
            await _bookingClient.DropBookingAsync(bookingId);
        }
        catch (Exception exception)
        {
            throw new BookingNotDropException($"Booking Id = {bookingId}", exception);
        }

        try
        {
            var orderUpdate = new OrderUpdate(Status.PaymentOverdue);
            _orderRepository.Update(order.Id, orderUpdate);
        }
        catch (Exception exception)
        {
            throw new OrderNotUpdatedException("", exception);
        }
    }

    private async Task WaitingForCaptureAsync(string paymentId)
    {
        var order = GetOrderByPaymentId(paymentId);

        await _bookingClient.ConfirmBookingAsync(order.BookingId);

        var orderUpdate = new OrderUpdate(Status.WaitingReceiving);
        _orderRepository.Update(order.Id, orderUpdate);
    }

    private OrderBrief GetOrderByPaymentId(string paymentId)
    {
        var order = _orderRepository.GetAll().SingleOrDefault(o => o.PaymentId == paymentId);
        return order ?? throw new OrderNotFoundException($"Order with Payment Id = {paymentId}");
    }
}