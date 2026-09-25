using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents an apple available for purchase.
/// </summary>
public sealed class Apple : IFruit
{
    /// <inheritdoc />
    public int Id => 101;

    /// <inheritdoc />
    public string Name => "Apple";

    /// <inheritdoc />
    public decimal Price => 200m;

    /// <inheritdoc />
    public string Emoji => "🍎";
}