namespace InventoryService.Domain.Entities;

public class InventoryItem
{
    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public int AvailableQuantity { get; private set; }

    private InventoryItem()
    {
    }

    public InventoryItem(
        Guid productId,
        int availableQuantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException(
                "ProductId is required.");

        if (availableQuantity < 0)
            throw new ArgumentException(
                "Quantity cannot be negative.");

        Id = Guid.NewGuid();
        ProductId = productId;
        AvailableQuantity = availableQuantity;
    }

    public bool Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        if (AvailableQuantity < quantity)
            return false;

        AvailableQuantity -= quantity;

        return true;
    }
}