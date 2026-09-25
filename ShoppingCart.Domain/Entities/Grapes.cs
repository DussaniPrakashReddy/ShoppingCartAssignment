using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents grapes available for purchase.
/// </summary>
public sealed class Grapes : IFruit
{
    public int Id => 103;

    public string Name => "Grapes";

    public decimal Price => 120m;

    public string Emoji => "🍇";
}