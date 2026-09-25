using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.Services.Interfaces;

/// <summary>
/// Provides fruit-related application operations.
/// </summary>
public interface IFruitService
{
    /// <summary>
    /// Gets all fruits available for purchase.
    /// </summary>
    IReadOnlyCollection<IFruit> GetAvailableFruits();
}