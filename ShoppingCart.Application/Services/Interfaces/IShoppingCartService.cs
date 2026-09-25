using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.Services.Interfaces;

/// <summary>
/// Defines operations for managing the shopping cart.
/// </summary>
public interface IShoppingCartService
{
    /// <summary>
    /// Gets the current items in the cart.
    /// </summary>
    IReadOnlyCollection<CartItem> Items { get; }

    /// <summary>
    /// Gets the current total value of the cart.
    /// </summary>
    decimal Total { get; }

    /// <summary>
    /// Adds a fruit to the cart.
    /// If the fruit already exists, its quantity is incremented.
    /// </summary>
    /// <param name="fruit">Fruit to add.</param>
    void Add(IFruit fruit);

    /// <summary>
    /// Increases the quantity of a fruit already in the cart.
    /// </summary>
    /// <param name="fruit">Fruit whose quantity should increase.</param>
    void Increase(IFruit fruit);

    /// <summary>
    /// Decreases the quantity of a fruit in the cart.
    /// If the quantity reaches zero, the item is removed.
    /// </summary>
    /// <param name="fruit">Fruit whose quantity should decrease.</param>
    void Decrease(IFruit fruit);

    /// <summary>
    /// Removes a fruit completely from the cart.
    /// </summary>
    /// <param name="fruit">Fruit to remove.</param>
    void Remove(IFruit fruit);

    /// <summary>
    /// Removes all items from the cart.
    /// </summary>
    void Clear();
}