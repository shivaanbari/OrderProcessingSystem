using MassTransit;
using MediatR;
using OrderProcessing.Contracts.Events;
using OrderService.Application.Features.Orders.Commands;

namespace OrderService.API.Consumers;

public class InventoryReservedConsumer
    : IConsumer<InventoryReservedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<InventoryReservedConsumer> _logger;

    public InventoryReservedConsumer(
        IMediator mediator,
        ILogger<InventoryReservedConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<InventoryReservedEvent> context)
    {
        await _mediator.Send(
            new ProcessInventoryResultCommand(
                context.Message.OrderId,
                true),
            context.CancellationToken);

        _logger.LogInformation(
            "Order {OrderId} confirmed successfully.",
            context.Message.OrderId);
    }
}