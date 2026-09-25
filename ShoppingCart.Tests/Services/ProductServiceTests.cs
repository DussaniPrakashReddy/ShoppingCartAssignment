using NUnit.Framework;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Tests.Fakes;

namespace ShoppingCart.Tests.Services;

[TestFixture]
public sealed class ProductServiceTests
{
    private FakeProductRepository _repository = null!;
    private ProductService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new FakeProductRepository();

        _service = new ProductService(_repository);
    }

    [Test]
    public void CreateProduct_ValidProduct_CreatesProduct()
    {
        var product = _service.CreateProduct(
            "Apple",
            "Fruits",
            100m,
            "🍎");

        Assert.That(product, Is.Not.Null);
        Assert.That(product.Id, Is.EqualTo(101));
        Assert.That(product.Name, Is.EqualTo("Apple"));
        Assert.That(product.Category, Is.EqualTo("Fruits"));
        Assert.That(product.Price, Is.EqualTo(100m));
        Assert.That(product.Emoji, Is.EqualTo("🍎"));
        Assert.That(product.IsAvailable, Is.True);
    }

    [Test]
    public void CreateProduct_ValidProduct_TrimsValues()
    {
        var product = _service.CreateProduct(
            " Apple ",
            " Fruits ",
            100m,
            " 🍎 ");

        Assert.That(product.Name, Is.EqualTo("Apple"));
        Assert.That(product.Category, Is.EqualTo("Fruits"));
        Assert.That(product.Emoji, Is.EqualTo("🍎"));
    }

    [Test]
    public void CreateProduct_EmptyName_ThrowsArgumentException()
    {
        Assert.That(
            () => _service.CreateProduct(
                string.Empty,
                "Fruits",
                100m,
                "🍎"),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void CreateProduct_WhitespaceName_ThrowsArgumentException()
    {
        Assert.That(
            () => _service.CreateProduct(
                "   ",
                "Fruits",
                100m,
                "🍎"),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void CreateProduct_EmptyCategory_ThrowsArgumentException()
    {
        Assert.That(
            () => _service.CreateProduct(
                "Apple",
                string.Empty,
                100m,
                "🍎"),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void CreateProduct_NegativePrice_ThrowsArgumentOutOfRangeException()
    {
        Assert.That(
            () => _service.CreateProduct(
                "Apple",
                "Fruits",
                -1m,
                "🍎"),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void GetProducts_ReturnsAllProducts()
    {
        _repository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 100m,
                IsAvailable = true
            },
            new FruitProduct
            {
                Id = 102,
                Name = "Mango",
                Category = "Fruits",
                Price = 150m,
                IsAvailable = true
            });

        var products = _service.GetProducts();

        Assert.That(products, Has.Count.EqualTo(2));

        Assert.That(
            products.Select(product => product.Name),
            Is.EquivalentTo(new[]
            {
                "Apple",
                "Mango"
            }));
    }

    [Test]
    public void GetAvailableProducts_ReturnsOnlyAvailableProducts()
    {
        _repository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 100m,
                IsAvailable = true
            },
            new FruitProduct
            {
                Id = 102,
                Name = "Mango",
                Category = "Fruits",
                Price = 150m,
                IsAvailable = false
            });

        var products =
            _service.GetAvailableProducts();

        Assert.That(products, Has.Count.EqualTo(1));

        Assert.That(
            products.Single().Name,
            Is.EqualTo("Apple"));
    }

    [Test]
    public void FindProduct_ExistingProduct_ReturnsProduct()
    {
        _repository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 100m,
                IsAvailable = true
            });

        var product =
            _service.FindProduct(101);

        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Id, Is.EqualTo(101));
        Assert.That(product.Name, Is.EqualTo("Apple"));
    }

    [Test]
    public void FindProduct_InvalidId_ReturnsNull()
    {
        var product =
            _service.FindProduct(999);

        Assert.That(product, Is.Null);
    }

    [Test]
    public void UpdateProduct_ChangesPrice()
    {
        _repository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 200m,
                IsAvailable = true
            });

        var product =
            _repository.FindById(101)!;

        product.Price = 100m;

        _service.UpdateProduct(product);

        var updated =
            _repository.FindById(101);

        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Price, Is.EqualTo(100m));
    }

    [Test]
    public void UpdateProduct_ChangesAvailability()
    {
        _repository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 200m,
                IsAvailable = true
            });

        var product =
            _repository.FindById(101)!;

        product.IsAvailable = false;

        _service.UpdateProduct(product);

        var updated =
            _repository.FindById(101);

        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.IsAvailable, Is.False);
    }

    [Test]
    public void DeleteProduct_ExistingProduct_RemovesProduct()
    {
        _repository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 100m,
                IsAvailable = true
            });

        _service.DeleteProduct(101);

        var product =
            _repository.FindById(101);

        Assert.That(product, Is.Null);
    }

    [Test]
    public void DeleteProduct_InvalidId_ThrowsArgumentOutOfRangeException()
    {
        Assert.That(
            () => _service.DeleteProduct(0),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }
}