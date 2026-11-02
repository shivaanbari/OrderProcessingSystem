using OrderService.Domain.Entities;

namespace OrderService.Application.Abstractions;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);

    Task<Order?> GetByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken);
    Task UpdateAsync(
    Order order,
    CancellationToken cancellationToken);
}