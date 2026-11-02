using InventoryService.Worker;
using InventoryService.Application.Abstractions;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using InventoryService.Application.Features.Inventory.Commands;
using InventoryService.Worker.Consumers;
using MediatR;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("InventoryDb")));
builder.Services.AddMediatR(
    typeof(ReserveInventoryCommand).Assembly);
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitConfig =
            builder.Configuration.GetSection("RabbitMQ");

        cfg.Host(
            rabbitConfig["Host"],
            "/",
            h =>
            {
                h.Username(rabbitConfig["Username"]!);
                h.Password(rabbitConfig["Password"]!);
            });

        cfg.ConfigureEndpoints(context);
    });
});
var host = builder.Build();
host.Run();
