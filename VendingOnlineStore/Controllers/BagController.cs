using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Clients.Payment;

namespace VendingOnlineStore.Controllers;

public class BagController : Controller
{
    private readonly IPaymentClient _paymentClient;

    public BagController(IPaymentClient paymentClient)
    {
        _paymentClient = paymentClient;
    }

    public async Task<IActionResult> Buy(string id)
    {
        var url = await _paymentClient.CreatePayment();
        return Redirect(url);
    }

    public async Task<IActionResult> BuyItem(decimal price)
    {
        var url = await _paymentClient.CreatePayment(price);
        return Redirect(url);
    }
}
