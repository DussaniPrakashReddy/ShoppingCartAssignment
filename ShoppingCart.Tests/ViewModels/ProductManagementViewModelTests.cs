using NUnit.Framework;
using ShoppingCart.Application.Services;
using ShoppingCart.Tests.Fakes;
using ShoppingCart.WPF.ViewModels;

namespace ShoppingCart.Tests.ViewModels;

[TestFixture]
public sealed class ProductManagementViewModelTests
{
    private FakeProductRepository _repository = null!;
    private ProductManagementViewModel _viewModel = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new FakeProductRepository();

        _repository.Seed(
            new()
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 200m,
                Emoji = "🍎",
                IsAvailable = true
            },
            new()
            {
                Id = 102,
                Name = "Mango",
                Category = "Fruits",
                Price = 150m,
                Emoji = "🥭",
                IsAvailable = true
            });

        var service =
            new ProductService(_repository);

        _viewModel =
            new ProductManagementViewModel(service);
    }

    [Test]
    public void Constructor_LoadsProducts()
    {
        Assert.That(
            _viewModel.Products,
            Has.Count.EqualTo(2));
    }

    [Test]
    public void SearchText_FiltersProductsByName()
    {
        _viewModel.SearchText = "Apple";

        Assert.That(
            _viewModel.Products,
            Has.Count.EqualTo(1));

        Assert.That(
            _viewModel.Products.Single().Name,
            Is.EqualTo("Apple"));
    }

    [Test]
    public void SearchText_FiltersProductsByCategory()
    {
        _viewModel.SearchText = "Fruits";

        Assert.That(
            _viewModel.Products,
            Has.Count.EqualTo(2));
    }

    [Test]
    public void AddProductCommand_StartsAddMode()
    {
        _viewModel.AddProductCommand.Execute(null);

        Assert.That(
            _viewModel.IsEditMode,
            Is.True);

        Assert.That(
            _viewModel.SelectedProduct,
            Is.Null);

        Assert.That(
            _viewModel.ProductName,
            Is.Empty);
    }

    [Test]
    public void SaveProduct_InAddMode_CreatesProduct()
    {
        _viewModel.AddProductCommand.Execute(null);

        _viewModel.ProductName = "Banana";
        _viewModel.Category = "Fruits";
        _viewModel.Price = 80m;
        _viewModel.Emoji = "🍌";
        _viewModel.IsAvailable = true;

        _viewModel.SaveProductCommand.Execute(null);

        var product =
            _repository.FindById(103);

        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Name, Is.EqualTo("Banana"));
        Assert.That(product.Price, Is.EqualTo(80m));
    }

    [Test]
    public void EditProduct_LoadsProductIntoEditor()
    {
        var product =
            _repository.FindById(101)!;

        _viewModel.EditProductCommand.Execute(product);

        Assert.That(
            _viewModel.IsEditMode,
            Is.True);

        Assert.That(
            _viewModel.ProductName,
            Is.EqualTo("Apple"));

        Assert.That(
            _viewModel.Price,
            Is.EqualTo(200m));
    }

    [Test]
    public void SaveProduct_InEditMode_UpdatesPrice()
    {
        var product =
            _repository.FindById(101)!;

        _viewModel.EditProductCommand.Execute(product);

        _viewModel.Price = 100m;

        _viewModel.SaveProductCommand.Execute(null);

        var updated =
            _repository.FindById(101);

        Assert.That(
            updated!.Price,
            Is.EqualTo(100m));
    }

    [Test]
    public void DeleteProduct_RemovesProduct()
    {
        var product =
            _repository.FindById(101)!;

        _viewModel.DeleteProductCommand.Execute(product);

        Assert.That(
            _repository.FindById(101),
            Is.Null);
    }

    [Test]
    public void ToggleAvailability_ChangesAvailability()
    {
        var product =
            _repository.FindById(101)!;

        Assert.That(product.IsAvailable, Is.True);

        _viewModel.ToggleAvailabilityCommand.Execute(product);

        var updated =
            _repository.FindById(101);

        Assert.That(
            updated!.IsAvailable,
            Is.False);
    }

    [Test]
    public void CancelCommand_ExitsEditMode()
    {
        _viewModel.AddProductCommand.Execute(null);

        Assert.That(
            _viewModel.IsEditMode,
            Is.True);

        _viewModel.CancelCommand.Execute(null);

        Assert.That(
            _viewModel.IsEditMode,
            Is.False);
    }
}