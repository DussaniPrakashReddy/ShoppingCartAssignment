using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents a banana available for purchase.
/// </summary>
public sealed class Banana : IFruit
{
    public int Id => 104;

    public string Name => "Banana";

    public decimal Price => 80m;

    public string Emoji => "🍌";
}