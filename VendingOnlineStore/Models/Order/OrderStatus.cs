namespace VendingOnlineStore.Models.Order;

public enum OrderStatus
{
    WaitingPayment,
    WaitingReceiving,
    PaymentOverdue,
    Received,
    ReceivingOverdue
}