using Core.Clients.Vending;
using Core.Repositories.Order;

namespace Core.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly IVendingClient _vendingClient;
    private readonly IOrderRepository _orderRepository;

    public PaymentService(IVendingClient vendingClient, IOrderRepository orderRepository)
    {
        _vendingClient = vendingClient;
        _orderRepository = orderRepository;
    }

    public void Succeeded(string paymentId)
    {
        throw new NotImplementedException();
    }

    public async Task Canceled(string paymentId)
    {
        var order = GetOrderByPaymentId(paymentId);

        var bookingId = order.BookingId;
        try
        {
            await _vendingClient.DropBookingAsync(bookingId);
        }
        catch (Exception exception)
        {
            throw new BookingNotDropException($"Booking Id = {bookingId}", exception);
        }

        try
        {
            _orderRepository.Update(order.Id, new UpdatedOrder());
        }
        catch (Exception exception)
        {
            throw new OrderNotUpdated(exception);
        }
    }

    public async Task WaitingForCapture(string paymentId)
    {
        var order = GetOrderByPaymentId(paymentId);

        var d = await _vendingClient.ConfirmBookingAsync(order.BookingId);

        var updatedOrderDetails = _orderRepository.Update(order.Id, new UpdatedOrder());
    }

    private OrderDto GetOrderByPaymentId(string paymentId)
    {
        var order = _orderRepository.GetAll().SingleOrDefault(o => o.PaymentId == paymentId);
        return order ?? throw new OrderNotFoundException($"Order with Payment Id = {paymentId}");
    }
}