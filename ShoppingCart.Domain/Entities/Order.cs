namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents a completed shopping transaction.
/// </summary>
public sealed class Order
{
    /// <summary>
    /// Gets the unique order identifier.
    /// </summary>
    public int OrderId { get; init; }

    /// <summary>
    /// Gets the identifier of the customer who placed the order.
    /// </summary>
    public int CustomerId { get; init; }

    /// <summary>
    /// Gets the date and time when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the products included in the order.
    /// </summary>
    public IReadOnlyCollection<CartItem> Items { get; init; }
        = Array.Empty<CartItem>();
}