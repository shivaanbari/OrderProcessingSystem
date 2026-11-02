using InventoryService.Domain.Entities;

namespace InventoryService.Application.Abstractions;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        InventoryItem item,
        CancellationToken cancellationToken);
}