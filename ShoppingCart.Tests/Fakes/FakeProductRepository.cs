using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Tests.Fakes;

/// <summary>
/// In-memory product repository used for unit testing.
/// </summary>
public sealed class FakeProductRepository : IProductRepository
{
    private readonly List<FruitProduct> _products = new();

    private int _lastProductId = 100;

    public IReadOnlyCollection<FruitProduct> GetAll()
    {
        return _products
            .Select(Clone)
            .ToList();
    }

    public int GetNextProductId()
    {
        return ++_lastProductId;
    }

    public FruitProduct Add(FruitProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _products.Add(Clone(product));

        if (product.Id > _lastProductId)
        {
            _lastProductId = product.Id;
        }

        return Clone(product);
    }

    public void Update(FruitProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var existingProduct = _products.FirstOrDefault(
            productItem => productItem.Id == product.Id);

        if (existingProduct is null)
        {
            throw new InvalidOperationException(
                $"Product with ID {product.Id} was not found.");
        }

        existingProduct.Name = product.Name;
        existingProduct.Category = product.Category;
        existingProduct.Price = product.Price;
        existingProduct.Emoji = product.Emoji;
        existingProduct.IsAvailable = product.IsAvailable;
    }

    public void Delete(int productId)
    {
        var product = _products.FirstOrDefault(
            productItem => productItem.Id == productId);

        if (product is not null)
        {
            _products.Remove(product);
        }
    }

    public FruitProduct? FindById(int productId)
    {
        var product = _products.FirstOrDefault(
            productItem => productItem.Id == productId);

        return product is null
            ? null
            : Clone(product);
    }

    public void Seed(params FruitProduct[] products)
    {
        foreach (var product in products)
        {
            Add(product);
        }
    }

    private static FruitProduct Clone(FruitProduct product)
    {
        return new FruitProduct
        {
            Id = product.Id,
            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            Emoji = product.Emoji,
            IsAvailable = product.IsAvailable
        };
    }
}