using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Services;

/// <summary>
/// Implements order-related application operations.
/// </summary>
public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="OrderService"/>.
    /// </summary>
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <inheritdoc />
    public int GetNextOrderId()
    {
        return _orderRepository.GetNextOrderId();
    }

    /// <inheritdoc />
    public void PlaceOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _orderRepository.Save(order);
    }
}