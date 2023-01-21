﻿using Core.Extensions;
using Core.Services.Order;
using Microsoft.AspNetCore.Mvc;
using VendingOnlineStore.Models.Order;

namespace VendingOnlineStore.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var orders = _orderService.GetAll();
        var model = orders.Select(o => MapToOrderViewModel(o));
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] NewOrderViewModel newOrderModel)
    {
        var newOrder = MapToNewOrder(newOrderModel);
        var paymentLink = await _orderService.CreateAsync(newOrder);
        return Redirect(paymentLink);
    }

    [HttpGet]
    public IActionResult Details(Guid id)
    {
        var orderDetails = _orderService.GetByIdOrDefault(id);
        if (orderDetails == default)
            return NotFound();

        var model = MapToOrderDetailsViewModel(orderDetails);
        return View(model);
    }

    private static OrderViewModel MapToOrderViewModel(OrderBrief order)
    {
        var orderViewModel = new OrderViewModel(order.Id, order.CreationDateUtc, order.Status);
        return orderViewModel;
    }

    private static OrderDetailsViewModel MapToOrderDetailsViewModel(OrderDetails orderDetails)
    {
        var pickupPointViewModel = MapToOrderPickupPointViewModel(orderDetails.PickupPoint);
        var orderContentsViewModels = orderDetails.Contents.Select(MapToOrderContentViewModel);
        var orderDetailsViewModel = new OrderDetailsViewModel(orderDetails.CreationDateUtc, orderDetails.Status,
            orderDetails.PaymentLink, pickupPointViewModel, orderContentsViewModels, orderDetails.TotalPrice);
        return orderDetailsViewModel;
    }

    private static OrderPickupPointViewModel MapToOrderPickupPointViewModel(OrderPickupPoint orderPickupPoint)
    {
        var orderPickupPointViewModel = new OrderPickupPointViewModel(
            orderPickupPoint.Address,
            orderPickupPoint.Description
        );
        return orderPickupPointViewModel;
    }

    private static OrderContentViewModel MapToOrderContentViewModel(OrderContent orderContent)
    {
        var orderContentViewModel = new OrderContentViewModel(
            orderContent.Name,
            orderContent.Description,
            orderContent.PhotoLink,
            orderContent.Count,
            orderContent.Price
        );
        return orderContentViewModel;
    }

    private static NewOrder MapToNewOrder(NewOrderViewModel newOrderModel)
    {
        return new NewOrder(newOrderModel.BagSectionId,
            newOrderModel.Contents.Select(c => new NewOrderContent(c.BagContentId, c.Count)).ToReadOnlyCollection());
    }
}