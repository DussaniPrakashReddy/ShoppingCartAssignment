namespace ShoppingCart.Domain.Interfaces;

/// <summary>
/// Defines the contract for a fruit that can be sold in the store.
/// </summary>
public interface IFruit
{
    /// <summary>
    /// Gets the unique identifier of the fruit.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Gets the display name of the fruit.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the selling price of one unit.
    /// </summary>
    decimal Price { get; }

    /// <summary>
    /// Gets the visual representation used by the fruit card.
    /// </summary>
    string Emoji { get; }
}