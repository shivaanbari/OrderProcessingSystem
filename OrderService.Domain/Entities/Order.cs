using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order()
    {
    }

    public Order(List<OrderItem> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Order must have at least one item.");

        Id = Guid.NewGuid();
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        _items.AddRange(items);
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException(
                "Only pending orders can be confirmed.");

        Status = OrderStatus.Confirmed;
    }

    public void Fail()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException(
                "Only pending orders can be failed.");

        Status = OrderStatus.Failed;
    }
}