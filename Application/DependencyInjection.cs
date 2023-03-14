using Application.Clients;
using Application.DataSimulation;
using Application.Repositories;
using Application.Services;
using Core.Clients.Booking;
using Core.Clients.Catalog;
using Core.Clients.Geo;
using Core.Clients.Map;
using Core.Clients.Payment;
using Core.Clients.PickupPoint;
using Core.Clients.Vending;
using Core.Repositories.BagContent;
using Core.Repositories.BagSection;
using Core.Repositories.Order;
using Core.Repositories.User;
using Core.Services.Account;
using Core.Services.Bag;
using Core.Services.Booking;
using Core.Services.Catalog;
using Core.Services.Checkout;
using Core.Services.Manage;
using Core.Services.Map;
using Core.Services.Order;
using Core.Services.Payment;
using Core.Services.Vending;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddClients();
        services.AddRepositories();
        services.AddServices();
        return services;
    }

    public static IServiceCollection AddPaymentApplication(this IServiceCollection services)
    {
        services.AddSingleton<IBookingClient, DummyBookingClient>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }

    public static IServiceCollection AddBookingApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentClient, YandexPaymentClient>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBookingService, BookingService>();
        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddSingleton<IVendingClient, DummyVendingClient>();
        services.AddSingleton<IPickupPointClient, DummyPickupPointClient>();
        services.AddSingleton<IBookingClient, DummyBookingClient>();
        services.AddSingleton<ICatalogClient, DummyCatalogClient>();
        services.AddSingleton<IPaymentClient, YandexPaymentClient>();
        services.AddSingleton<IMapClient, MapClient>();
        services.AddHttpClient<IMapClient, MapClient>();
        services.AddScoped<IGeoClient, GeoClient>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBagSectionRepository, BagSectionRepository>();
        services.AddScoped<IBagContentRepository, BagContentRepository>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IManageService, ManageService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IMapService, MapService>();
        services.AddScoped<IBagService, BagService>();
        services.AddScoped<IVendingService, VendingService>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}