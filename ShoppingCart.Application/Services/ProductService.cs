using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.Services;

/// <summary>
/// Implements product catalog business operations.
/// </summary>
public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="ProductService"/>.
    /// </summary>
    public ProductService(IProductRepository productRepository)
    {
        _productRepository =
            productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    }

    /// <inheritdoc />
    public IReadOnlyCollection<FruitProduct> GetProducts()
    {
        return _productRepository.GetAll();
    }

    /// <inheritdoc />
    public FruitProduct CreateProduct(
        string name,
        string category,
        decimal price,
        string emoji)
    {
        ValidateProduct(name, category, price);

        var product = new FruitProduct
        {
            Id = _productRepository.GetNextProductId(),
            Name = name.Trim(),
            Category = category.Trim(),
            Price = price,
            Emoji = emoji?.Trim() ?? string.Empty,
            IsAvailable = true
        };

        return _productRepository.Add(product);
    }

    /// <inheritdoc />
    public void UpdateProduct(FruitProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        ValidateProduct(
            product.Name,
            product.Category,
            product.Price);

        _productRepository.Update(product);
    }

    /// <inheritdoc />
    public void DeleteProduct(int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(productId),
                "Product ID must be greater than zero.");
        }

        _productRepository.Delete(productId);
    }

    /// <inheritdoc />
    public FruitProduct? FindProduct(int productId)
    {
        if (productId <= 0)
        {
            return null;
        }

        return _productRepository.FindById(productId);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IFruit> GetAvailableProducts()
    {
        return _productRepository
            .GetAll()
            .Where(product => product.IsAvailable)
            .Cast<IFruit>()
            .ToList();
    }

    private static void ValidateProduct(
        string name,
        string category,
        decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Product name is required.",
                nameof(name));
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException(
                "Product category is required.",
                nameof(category));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(price),
                "Product price cannot be negative.");
        }
    }
}