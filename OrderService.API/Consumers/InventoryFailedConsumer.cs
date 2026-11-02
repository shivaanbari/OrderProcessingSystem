using MassTransit;
using MediatR;
using OrderProcessing.Contracts.Events;
using OrderService.Application.Features.Orders.Commands;

namespace OrderService.API.Consumers;

public class InventoryFailedConsumer
    : IConsumer<InventoryFailedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<InventoryFailedConsumer> _logger;

    public InventoryFailedConsumer(
        IMediator mediator,
        ILogger<InventoryFailedConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<InventoryFailedEvent> context)
    {
        await _mediator.Send(
            new ProcessInventoryResultCommand(
                context.Message.OrderId,
                false),
            context.CancellationToken);

        _logger.LogWarning(
            "Order {OrderId} failed. Reason: {Reason}",
            context.Message.OrderId,
            context.Message.Reason);
    }
}