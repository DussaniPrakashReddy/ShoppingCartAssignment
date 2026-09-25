using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents a configurable fruit product in the product catalog.
/// </summary>
public sealed class FruitProduct : IFruit
{
    /// <summary>
    /// Gets or sets the unique product identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the selling price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the emoji representation of the product.
    /// </summary>
    public string Emoji { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product category.
    /// </summary>
    public string Category { get; set; } = "Fruits";

    /// <summary>
    /// Gets or sets a value indicating whether the product
    /// is currently available for sale.
    /// </summary>
    public bool IsAvailable { get; set; } = true;
}