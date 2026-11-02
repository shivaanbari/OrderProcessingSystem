using MediatR;
using OrderService.Domain.Enums;

namespace OrderService.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId)
    : IRequest<OrderDto?>;

public record OrderDto(
    Guid Id,
    OrderStatus Status,
    DateTime CreatedAt,
    List<OrderItemDto> Items
);

public record OrderItemDto(
    Guid ProductId,
    int Quantity
);