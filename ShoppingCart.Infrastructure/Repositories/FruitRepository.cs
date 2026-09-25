using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Infrastructure.Repositories;

/// <summary>
/// Provides product catalog information.
/// </summary>
public sealed class FruitRepository : IFruitRepository
{
    private readonly IReadOnlyCollection<IFruit> _products =
    [
        new FruitProduct
        {
            Id = 101,
            Name = "Apple",
            Price = 200m,
            Emoji = "🍎",
            Category = "Fruits",
            IsAvailable = true
        },

        new FruitProduct
        {
            Id = 102,
            Name = "Mango",
            Price = 150m,
            Emoji = "🥭",
            Category = "Fruits",
            IsAvailable = true
        },

        new FruitProduct
        {
            Id = 103,
            Name = "Grapes",
            Price = 120m,
            Emoji = "🍇",
            Category = "Fruits",
            IsAvailable = true
        },

        new FruitProduct
        {
            Id = 104,
            Name = "Banana",
            Price = 80m,
            Emoji = "🍌",
            Category = "Fruits",
            IsAvailable = true
        }
    ];

    /// <inheritdoc />
    public IReadOnlyCollection<IFruit> GetAll()
    {
        return _products
            .Where(product =>
                product is FruitProduct fruit &&
                fruit.IsAvailable)
            .ToList();
    }
}