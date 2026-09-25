using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Services.Interfaces;

/// <summary>
/// Provides order-related application operations.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Gets the next order identifier.
    /// </summary>
    int GetNextOrderId();

    /// <summary>
    /// Completes and persists the specified order.
    /// </summary>
    void PlaceOrder(Order order);
}