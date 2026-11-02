using MediatR;

namespace OrderService.Application.Features.Orders.Commands;

public record CreateOrderCommand(
    List<CreateOrderItemDto> Items
) : IRequest<Guid>;

public record CreateOrderItemDto(
    Guid ProductId,
    int Quantity
);