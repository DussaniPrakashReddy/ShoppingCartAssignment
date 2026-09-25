using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Interfaces;

/// <summary>
/// Defines persistence operations for the product catalog.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets all products in the catalog.
    /// </summary>
    IReadOnlyCollection<FruitProduct> GetAll();

    /// <summary>
    /// Gets the next available product identifier.
    /// </summary>
    int GetNextProductId();

    /// <summary>
    /// Adds a new product to the catalog.
    /// </summary>
    FruitProduct Add(FruitProduct product);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    void Update(FruitProduct product);

    /// <summary>
    /// Deletes a product from the catalog.
    /// </summary>
    void Delete(int productId);

    /// <summary>
    /// Finds a product by its identifier.
    /// </summary>
    FruitProduct? FindById(int productId);
}