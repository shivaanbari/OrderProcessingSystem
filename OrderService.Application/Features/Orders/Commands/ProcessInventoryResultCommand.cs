using MediatR;

namespace OrderService.Application.Features.Orders.Commands;

public record ProcessInventoryResultCommand(
    Guid OrderId,
    bool IsReserved
) : IRequest<Unit>;