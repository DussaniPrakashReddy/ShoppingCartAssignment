using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.Services;

/// <summary>
/// Provides the business logic for managing shopping cart items.
/// </summary>
public sealed class ShoppingCartService : IShoppingCartService
{
    private readonly List<CartItem> _items = new();


    /// <summary>
    /// Gets the items currently contained in the cart.
    /// </summary>
    public IReadOnlyCollection<CartItem> Items =>
        _items.AsReadOnly();


    /// <summary>
    /// Gets the total value of all items in the cart.
    /// </summary>
    public decimal Total =>
        _items.Sum(item => item.LineTotal);


    /// <summary>
    /// Adds a fruit to the cart.
    /// If the fruit already exists, its quantity is increased.
    /// </summary>
    /// <param name="fruit">
    /// Fruit to add.
    /// </param>
    public void Add(IFruit fruit)
    {
        ArgumentNullException.ThrowIfNull(fruit);

        var existingItem = FindItem(fruit);

        if (existingItem is not null)
        {
            existingItem.Quantity++;
            return;
        }

        _items.Add(
            new CartItem
            {
                Fruit = fruit,
                Quantity = 1
            });
    }


    /// <summary>
    /// Increases the quantity of an existing cart item.
    /// </summary>
    /// <param name="fruit">
    /// Fruit whose quantity should be increased.
    /// </param>
    public void Increase(IFruit fruit)
    {
        ArgumentNullException.ThrowIfNull(fruit);

        var item = FindItem(fruit);

        if (item is null)
        {
            throw new InvalidOperationException(
                "The fruit does not exist in the cart.");
        }

        item.Quantity++;
    }


    /// <summary>
    /// Decreases the quantity of an existing cart item.
    /// If the quantity reaches zero, the item is removed.
    /// </summary>
    /// <param name="fruit">
    /// Fruit whose quantity should be decreased.
    /// </param>
    public void Decrease(IFruit fruit)
    {
        ArgumentNullException.ThrowIfNull(fruit);

        var item = FindItem(fruit);

        if (item is null)
        {
            throw new InvalidOperationException(
                "The fruit does not exist in the cart.");
        }

        item.Quantity--;

        if (item.Quantity <= 0)
        {
            _items.Remove(item);
        }
    }


    /// <summary>
    /// Removes a fruit completely from the cart.
    /// </summary>
    /// <param name="fruit">
    /// Fruit to remove.
    /// </param>
    public void Remove(IFruit fruit)
    {
        ArgumentNullException.ThrowIfNull(fruit);

        var item = FindItem(fruit);

        if (item is not null)
        {
            _items.Remove(item);
        }
    }


    /// <summary>
    /// Removes all items from the cart.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }


    /// <summary>
    /// Finds an existing cart item using the fruit's
    /// business identifier.
    /// </summary>
    /// <param name="fruit">
    /// Fruit to locate.
    /// </param>
    /// <returns>
    /// The matching cart item, or <see langword="null"/>
    /// when the fruit is not present.
    /// </returns>
    private CartItem? FindItem(IFruit fruit)
    {
        return _items.FirstOrDefault(
            item => item.Fruit.Id == fruit.Id);
    }
}