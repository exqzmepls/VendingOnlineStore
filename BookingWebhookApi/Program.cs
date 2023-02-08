using Core.Clients.Payment;
using Core.Repositories.Order;
using Core.Services.Booking;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddSingleton<IPaymentClient, YandexPaymentClient>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IBookingService, BookingService>();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();