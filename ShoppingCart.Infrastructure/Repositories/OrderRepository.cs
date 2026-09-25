using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Repositories;

/// <summary>
/// Provides order persistence for the shopping cart application.
/// 
/// This initial implementation keeps the latest order identifier
/// in memory. Persistence can later be moved to XML, JSON, or SQL.
/// </summary>
public sealed class OrderRepository : IOrderRepository
{
    private int _lastOrderId = 1000;

    /// <inheritdoc />
    public int GetNextOrderId()
    {
        return ++_lastOrderId;
    }

    /// <inheritdoc />
    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        // Persistence will be implemented in the next phase.
    }
}