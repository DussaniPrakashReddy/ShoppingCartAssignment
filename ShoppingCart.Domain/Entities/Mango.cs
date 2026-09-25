using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents a mango available for purchase.
/// </summary>
public sealed class Mango : IFruit
{
    public int Id => 102;

    public string Name => "Mango";

    public decimal Price => 150m;

    public string Emoji => "🥭";
}