using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.Interfaces;

/// <summary>
/// Defines data-access operations for retrieving fruits.
/// </summary>
public interface IFruitRepository
{
    /// <summary>
    /// Retrieves all fruits available for sale.
    /// </summary>
    IReadOnlyCollection<IFruit> GetAll();
}