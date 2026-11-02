namespace OrderProcessing.Contracts.Events;

public record InventoryFailedEvent(
    Guid OrderId,
    string Reason
);