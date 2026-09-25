using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Tests.Fakes;

/// <summary>
/// In-memory order repository used for unit testing.
/// </summary>
public sealed class FakeOrderRepository : IOrderRepository
{
    private int _lastOrderId = 1000;

    public List<Order> SavedOrders { get; } = new();

    public int GetNextOrderId()
    {
        return ++_lastOrderId;
    }

    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        SavedOrders.Add(order);
    }
}