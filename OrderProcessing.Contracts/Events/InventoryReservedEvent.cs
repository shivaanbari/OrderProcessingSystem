namespace OrderProcessing.Contracts.Events;

public record InventoryReservedEvent(
    Guid OrderId
);