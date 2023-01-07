using Infrastructure;
using Microsoft.EntityFrameworkCore;
using VendingOnlineStore.Clients.Geo;
using VendingOnlineStore.Clients.Payment;
using VendingOnlineStore.Clients.Vending;
using VendingOnlineStore.Repositories.BagItem;
using VendingOnlineStore.Repositories.BagMachine;
using VendingOnlineStore.Services.Bag;
using VendingOnlineStore.Services.Vending;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// db context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// clients
services.AddSingleton<IVendingClient, DummyVendingClient>();
services.AddSingleton<IPaymentClient, YandexPaymentClient>();
services.AddScoped<IGeoClient, GeoClient>();

// repositories
services.AddScoped<IBagMachineRepository, BagMachineRepository>();
services.AddScoped<IBagItemRepository, BagItemRepository>();

// services
services.AddScoped<IBagService, BagService>();
services.AddScoped<IVendingService, VendingService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
