using Application;
using Infrastructure;
using VendingOnlineStore.Identity;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// db context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddInfrastructure(connectionString);

services.AddApplication();

services.AddAspNetCoreIdentity();

services.AddHttpContextAccessor();
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