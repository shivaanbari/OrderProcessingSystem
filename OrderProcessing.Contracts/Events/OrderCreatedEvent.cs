namespace OrderProcessing.Contracts.Events;

public record OrderCreatedEvent(
    Guid OrderId,
    List<OrderItemMessage> Items
);

public record OrderItemMessage(
    Guid ProductId,
    int Quantity
);