using Microsoft.EntityFrameworkCore;
using OrderService.Application.Abstractions;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Order order,
        CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(
            order,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == orderId,
                cancellationToken);
    }
    public async Task UpdateAsync(
    Order order,
    CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);

        await _context.SaveChangesAsync(cancellationToken);
    }
}