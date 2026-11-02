using MediatR;
using OrderService.Application.Features.Orders.Commands;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories;
using MassTransit;
using OrderService.Infrastructure.Messaging;
using OrderService.API.Consumers;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(
    typeof(CreateOrderCommand).Assembly
);
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("OrderDb"));
});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<InventoryReservedConsumer>();
    x.AddConsumer<InventoryFailedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitConfig =
            builder.Configuration.GetSection("RabbitMQ");

        cfg.Host(
            rabbitConfig["Host"],
            rabbitConfig["VirtualHost"],
            h =>
            {
                h.Username(rabbitConfig["Username"]!);
                h.Password(rabbitConfig["Password"]!);
            });

        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddScoped<
    IIntegrationEventPublisher,
    MassTransitEventPublisher>();

builder.Services.AddScoped<IEventBus, MassTransitEventBus>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
