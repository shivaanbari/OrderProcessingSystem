using InventoryService.Application.Abstractions;
using MediatR;

namespace InventoryService.Application.Features.Inventory.Commands;

public class ReserveInventoryCommandHandler
    : IRequestHandler<ReserveInventoryCommand, bool>
{
    private readonly IInventoryRepository _repository;

    public ReserveInventoryCommandHandler(
        IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
        ReserveInventoryCommand request,
        CancellationToken cancellationToken)
    {
        var reservedItems = new List<(
            InventoryService.Domain.Entities.InventoryItem Item,
            int Quantity)>();

        foreach (var requestedItem in request.Items)
        {
            var inventory = await _repository.GetByProductIdAsync(
                requestedItem.ProductId,
                cancellationToken);

            if (inventory == null ||
                inventory.AvailableQuantity < requestedItem.Quantity)
            {
                return false;
            }

            reservedItems.Add((inventory, requestedItem.Quantity));
        }

        // فقط اگر موجودی همه محصولات کافی بود، رزرو انجام می‌شود
        foreach (var reservedItem in reservedItems)
        {
            reservedItem.Item.Reserve(reservedItem.Quantity);

            await _repository.UpdateAsync(
                reservedItem.Item,
                cancellationToken);
        }

        return true;
    }
}