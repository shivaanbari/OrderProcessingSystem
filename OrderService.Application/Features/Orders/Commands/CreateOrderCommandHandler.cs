using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Application.Features.Orders.Commands;
using OrderService.Domain.Entities;
using OrderProcessing.Contracts.Events;

namespace OrderService.Application.Features.Orders.Commands;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IIntegrationEventPublisher _eventPublisher;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IIntegrationEventPublisher eventPublisher)
    {
        _orderRepository = orderRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var items = request.Items
            .Select(item => new OrderItem(item.ProductId, item.Quantity))
            .ToList();

        var order = new Order(items);

        await _orderRepository.AddAsync(order, cancellationToken);

        var orderCreatedEvent = new OrderCreatedEvent(
            order.Id,
            order.Items.Select(item =>
                new OrderItemMessage(item.ProductId, item.Quantity))
                .ToList()
        );

        await _eventPublisher.PublishAsync(
            orderCreatedEvent,
            cancellationToken);

        return order.Id;
    }
}