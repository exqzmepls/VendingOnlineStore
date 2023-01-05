using VendingOnlineStore.Clients.Geo;
using VendingOnlineStore.Clients.Payment;
using VendingOnlineStore.Clients.Vending;
using VendingOnlineStore.Services.Bag;
using VendingOnlineStore.Services.Vending;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddSingleton<IVendingClient, DummyVendingClient>();
services.AddSingleton<IPaymentClient, YandexPaymentClient>();
services.AddSingleton<IBagService, DummyBagService>();
services.AddScoped<IGeoClient, GeoClient>();
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
