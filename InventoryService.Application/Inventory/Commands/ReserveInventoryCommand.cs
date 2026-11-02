using MediatR;

namespace InventoryService.Application.Features.Inventory.Commands;

public record ReserveInventoryCommand(
    Guid OrderId,
    List<ReserveInventoryItemDto> Items
) : IRequest<bool>;

public record ReserveInventoryItemDto(
    Guid ProductId,
    int Quantity
);