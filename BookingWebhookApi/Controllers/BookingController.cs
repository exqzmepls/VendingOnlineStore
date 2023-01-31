using System.ComponentModel.DataAnnotations;
using BookingWebhookApi.Contracts;
using Core.Services.Booking;
using Microsoft.AspNetCore.Mvc;

namespace BookingWebhookApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingController> _logger;

    public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [HttpPost(nameof(Release))]
    public async Task<IActionResult> Release([Required, FromBody] ReleaseNotification notification)
    {
        var booking = notification.Booking;
        var bookingId = booking.Id;
        try
        {
            await _bookingService.OnReleaseAsync(bookingId);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Release webhook failed.");
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost(nameof(Overdue))]
    public IActionResult Overdue([Required, FromBody] OverdueNotification notification)
    {
        var booking = notification.Booking;
        var bookingId = booking.Id;
        try
        {
            _bookingService.OnOverdueAsync(bookingId);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Overdue webhook failed.");
            return BadRequest();
        }

        return Ok();
    }
}