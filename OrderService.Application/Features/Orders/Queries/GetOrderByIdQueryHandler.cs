using MediatR;
using OrderService.Application.Abstractions;

namespace OrderService.Application.Features.Orders.Queries;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdQueryHandler(
        IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderDto?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order == null)
            return null;

        return new OrderDto(
            order.Id,
            order.Status,
            order.CreatedAt,
            order.Items.Select(item =>
                new OrderItemDto(
                    item.ProductId,
                    item.Quantity
                )).ToList()
        );
    }
}