using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.Services.Interfaces;

/// <summary>
/// Defines application operations for managing products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Gets all products in the catalog.
    /// </summary>
    IReadOnlyCollection<FruitProduct> GetProducts();

    /// <summary>
    /// Creates a new product.
    /// </summary>
    FruitProduct CreateProduct(
        string name,
        string category,
        decimal price,
        string emoji);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    void UpdateProduct(FruitProduct product);

    /// <summary>
    /// Deletes a product.
    /// </summary>
    void DeleteProduct(int productId);

    /// <summary>
    /// Finds a product by identifier.
    /// </summary>
    FruitProduct? FindProduct(int productId);

    /// <summary>
    /// Gets products that are currently available for sale.
    /// </summary>
    IReadOnlyCollection<IFruit> GetAvailableProducts();
}