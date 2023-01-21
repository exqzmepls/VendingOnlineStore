namespace Infrastructure.Models;

public enum OrderStatus
{
    WaitingPayment,
    WaitingReceiving,
    PaymentOverdue,
    Received,
    ReceivingOverdue
}