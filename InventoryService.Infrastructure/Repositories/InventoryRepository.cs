using InventoryService.Application.Abstractions;
using InventoryService.Domain.Entities;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItem?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .FirstOrDefaultAsync(
                x => x.ProductId == productId,
                cancellationToken);
    }

    public async Task UpdateAsync(
        InventoryItem item,
        CancellationToken cancellationToken)
    {
        _context.InventoryItems.Update(item);

        await _context.SaveChangesAsync(cancellationToken);
    }
}