using Core.Repositories.Order;
using Core.Services.Order;
using OrderStatusEntity = Infrastructure.Models.OrderStatus;

namespace Application.Extensions;

internal static class EnumExtensions
{
    public static OrderStatus MapToOrderStatus(this OrderStatusData orderStatusData)
    {
        return orderStatusData switch
        {
            OrderStatusData.WaitingPayment => OrderStatus.WaitingPayment,
            OrderStatusData.WaitingReceiving => OrderStatus.WaitingReceiving,
            OrderStatusData.PaymentOverdue => OrderStatus.PaymentOverdue,
            OrderStatusData.Received => OrderStatus.Received,
            OrderStatusData.ReceivingOverdue => OrderStatus.ReceivingOverdue,
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatusData), orderStatusData, null)
        };
    }

    public static OrderStatusData MapToOrderStatusData(this OrderStatusEntity orderStatusEntity)
    {
        return orderStatusEntity switch
        {
            OrderStatusEntity.WaitingPayment => OrderStatusData.WaitingPayment,
            OrderStatusEntity.WaitingReceiving => OrderStatusData.WaitingReceiving,
            OrderStatusEntity.PaymentOverdue => OrderStatusData.PaymentOverdue,
            OrderStatusEntity.Received => OrderStatusData.Received,
            OrderStatusEntity.ReceivingOverdue => OrderStatusData.ReceivingOverdue,
            _ => throw new ArgumentOutOfRangeException(nameof(orderStatusEntity), orderStatusEntity, null)
        };
    }
}