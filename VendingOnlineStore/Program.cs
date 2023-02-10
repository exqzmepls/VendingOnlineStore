using Core.AppInterfaces;
using Core.Clients.Booking;
using Core.Clients.Catalog;
using Core.Clients.Geo;
using Core.Clients.Payment;
using Core.Clients.PickupPoint;
using Core.Clients.Vending;
using Core.DataSimulation;
using Core.Repositories.BagContent;
using Core.Repositories.BagSection;
using Core.Repositories.Order;
using Core.Services.Account;
using Core.Services.Bag;
using Core.Services.Catalog;
using Core.Services.Checkout;
using Core.Services.Order;
using Core.Services.Vending;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingOnlineStore.Implementations;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// db context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// identity
services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<AppDbContext>();

// clients
services.AddSingleton<IVendingClient, DummyVendingClient>();
services.AddSingleton<IPickupPointClient, DummyPickupPointClient>();
services.AddSingleton<IBookingClient, DummyBookingClient>();
services.AddSingleton<ICatalogClient, DummyCatalogClient>();
services.AddSingleton<IPaymentClient, YandexPaymentClient>();
services.AddScoped<IGeoClient, GeoClient>();

// implementations
services.AddScoped<ISignInManager<User>, NonPersistentSingInManager>();

// repositories
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IBagSectionRepository, BagSectionRepository>();
services.AddScoped<IBagContentRepository, BagContentRepository>();

// services
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<ICatalogService, CatalogService>();
services.AddScoped<IBagService, BagService>();
services.AddScoped<IVendingService, VendingService>();
services.AddScoped<ICheckoutService, CheckoutService>();
services.AddScoped<IOrderService, OrderService>();

services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();