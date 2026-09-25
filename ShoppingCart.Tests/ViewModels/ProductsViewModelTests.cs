using NUnit.Framework;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Tests.Fakes;
using ShoppingCart.WPF.ViewModels;

namespace ShoppingCart.Tests.ViewModels;

[TestFixture]
public sealed class ProductsViewModelTests
{
    private FakeProductRepository _repository = null!;
    private ProductService _service = null!;
    private ProductsViewModel _viewModel = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new FakeProductRepository();

        _repository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 100m,
                Emoji = "🍎",
                IsAvailable = true
            },
            new FruitProduct
            {
                Id = 102,
                Name = "Mango",
                Category = "Fruits",
                Price = 150m,
                Emoji = "🥭",
                IsAvailable = true
            },
            new FruitProduct
            {
                Id = 103,
                Name = "Grapes",
                Category = "Fruits",
                Price = 120m,
                Emoji = "🍇",
                IsAvailable = false
            });

        _service =
            new ProductService(_repository);

        _viewModel =
            new ProductsViewModel(_service);
    }

    [Test]
    public void Constructor_LoadsAvailableProductsOnly()
    {
        Assert.That(
            _viewModel.Products,
            Has.Count.EqualTo(2));

        Assert.That(
            _viewModel.Products.Select(product => product.Name),
            Is.EquivalentTo(new[]
            {
                "Apple",
                "Mango"
            }));
    }

    [Test]
    public void Constructor_DoesNotLoadUnavailableProducts()
    {
        Assert.That(
            _viewModel.Products.Any(
                product => product.Name == "Grapes"),
            Is.False);
    }

    [Test]
    public void Refresh_LoadsCurrentAvailableProducts()
    {
        var grapes =
            _repository.FindById(103)!;

        grapes.IsAvailable = true;

        _repository.Update(grapes);

        _viewModel.Refresh();

        Assert.That(
            _viewModel.Products,
            Has.Count.EqualTo(3));

        Assert.That(
            _viewModel.Products.Any(
                product => product.Name == "Grapes"),
            Is.True);
    }

    [Test]
    public void Refresh_RemovesProductsThatBecomeUnavailable()
    {
        var apple =
            _repository.FindById(101)!;

        apple.IsAvailable = false;

        _repository.Update(apple);

        _viewModel.Refresh();

        Assert.That(
            _viewModel.Products,
            Has.Count.EqualTo(1));

        Assert.That(
            _viewModel.Products.Single().Name,
            Is.EqualTo("Mango"));
    }

    [Test]
    public void Refresh_LoadsNewlyAddedProduct()
    {
        _repository.Add(
            new FruitProduct
            {
                Id = 104,
                Name = "Banana",
                Category = "Fruits",
                Price = 80m,
                Emoji = "🍌",
                IsAvailable = true
            });

        _viewModel.Refresh();

        Assert.That(
            _viewModel.Products,
            Has.Count.EqualTo(3));

        Assert.That(
            _viewModel.Products.Any(
                product => product.Name == "Banana"),
            Is.True);
    }
}