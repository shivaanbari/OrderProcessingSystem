using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Domain.Enums;

namespace OrderService.Application.Features.Orders.Commands;

public class ProcessInventoryResultCommandHandler
    : IRequestHandler<ProcessInventoryResultCommand, Unit>
{
    private readonly IOrderRepository _repository;

    public ProcessInventoryResultCommandHandler(
        IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(
        ProcessInventoryResultCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order == null)
            throw new InvalidOperationException(
                $"Order {request.OrderId} was not found.");

        if (order.Status != OrderStatus.Pending)
            return Unit.Value;

        if (request.IsReserved)
            order.Confirm();
        else
            order.Fail();

        await _repository.UpdateAsync(order, cancellationToken);

        return Unit.Value;
    }
}