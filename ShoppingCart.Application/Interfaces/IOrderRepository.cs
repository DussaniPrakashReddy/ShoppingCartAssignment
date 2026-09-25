using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Interfaces;

/// <summary>
/// Defines persistence operations for customer orders.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Gets the next available order identifier.
    /// </summary>
    /// <returns>
    /// The next unique order identifier.
    /// </returns>
    int GetNextOrderId();

    /// <summary>
    /// Persists the specified order.
    /// </summary>
    /// <param name="order">
    /// Order to persist.
    /// </param>
    void Save(Order order);
}