using InventoryService.Application.Abstractions;
using MassTransit;
using OrderProcessing.Contracts.Events;

namespace InventoryService.Worker.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IInventoryRepository _inventoryRepository;

    public OrderCreatedConsumer(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var orderEvent = context.Message;

        var requestedItems = orderEvent.Items
            .GroupBy(x => x.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                Quantity = group.Sum(x => x.Quantity)
            })
            .ToList();

        var itemsToReserve = new List<(Domain.Entities.InventoryItem Item, int Quantity)>();

        foreach (var requestedItem in requestedItems)
        {
            var inventoryItem =
                await _inventoryRepository.GetByProductIdAsync(
                    requestedItem.ProductId,
                    context.CancellationToken);

            if (inventoryItem == null ||
                inventoryItem.AvailableQuantity < requestedItem.Quantity)
            {
                await context.Publish(new InventoryFailedEvent(
                    orderEvent.OrderId,
                    $"Insufficient inventory for product {requestedItem.ProductId}."));

                return;
            }

            itemsToReserve.Add((inventoryItem, requestedItem.Quantity));
        }

        foreach (var entry in itemsToReserve)
        {
            var reserved = entry.Item.Reserve(entry.Quantity);

            if (!reserved)
            {
                await context.Publish(new InventoryFailedEvent(
                    orderEvent.OrderId,
                    $"Could not reserve product {entry.Item.ProductId}."));

                return;
            }

            await _inventoryRepository.UpdateAsync(
                entry.Item,
                context.CancellationToken);
        }

        await context.Publish(new InventoryReservedEvent(orderEvent.OrderId));
    }
}