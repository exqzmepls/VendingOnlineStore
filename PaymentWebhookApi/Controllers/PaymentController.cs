using System.ComponentModel.DataAnnotations;
using Core.Services.Payment;
using Microsoft.AspNetCore.Mvc;
using PaymentWebhookApi.Contracts;

namespace PaymentWebhookApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> WebhookAsync([FromBody, Required] WebhookNotification notification)
    {
        try
        {
            var eventName = notification.Event;
            var paymentId = notification.Object.Id;
            _logger.LogInformation("Webhook notification ({EventName} for payment {PaymentId})", eventName, paymentId);
            await _paymentService.ProcessEventAsync(eventName, paymentId);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "notification error");
            return BadRequest();
        }

        _logger.LogInformation("Webhook succeed");
        return Ok();
    }
}