using NUnit.Framework;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Tests.Fakes;
using ShoppingCart.WPF.ViewModels;

namespace ShoppingCart.Tests.ViewModels;

[TestFixture]
public sealed class ShoppingCartViewModelTests
{
    private FakeCustomerRepository _customerRepository = null!;
    private FakeProductRepository _productRepository = null!;
    private FakeOrderRepository _orderRepository = null!;

    private ShoppingCartViewModel _viewModel = null!;

    [SetUp]
    public void SetUp()
    {
        _customerRepository =
            new FakeCustomerRepository();

        _customerRepository.Seed(
            new Customer
            {
                Id = 1,
                Name = "Prakash",
                MobileNumber = "9876543210"
            });

        _productRepository =
            new FakeProductRepository();

        _productRepository.Seed(
            new FruitProduct
            {
                Id = 101,
                Name = "Apple",
                Category = "Fruits",
                Price = 100m,
                IsAvailable = true
            });

        _orderRepository =
            new FakeOrderRepository();

        var customerService =
            new CustomerService(
                _customerRepository);

        var productService =
            new ProductService(
                _productRepository);

        var orderService =
            new OrderService(
                _orderRepository);

        var shoppingCartService =
            new ShoppingCartService();

        var customerViewModel =
            new CustomerViewModel(
                customerService);

        var productsViewModel =
            new ProductsViewModel(
                productService);

        var navigationService =
            new FakeWindowNavigationService();

        _viewModel =
            new ShoppingCartViewModel(
                customerViewModel,
                productsViewModel,
                orderService,
                shoppingCartService,
                navigationService);
    }

    [Test]
    public void Constructor_LoadsAvailableProducts()
    {
        Assert.That(
            _viewModel.ProductsViewModel.Products,
            Has.Count.EqualTo(1));

        Assert.That(
            _viewModel.ProductsViewModel.Products.Single().Name,
            Is.EqualTo("Apple"));
    }

    [Test]
    public void AddToCartCommand_AddsProduct()
    {
        var product =
            _viewModel.ProductsViewModel.Products.Single();

        _viewModel.AddToCartCommand.Execute(product);

        Assert.That(
            _viewModel.HasItems,
            Is.True);

        Assert.That(
            _viewModel.CartItems,
            Has.Count.EqualTo(1));

        Assert.That(
            _viewModel.Total,
            Is.EqualTo(100m));
    }

    [Test]
    public void AddToCartCommand_Twice_IncreasesQuantity()
    {
        var product =
            _viewModel.ProductsViewModel.Products.Single();

        _viewModel.AddToCartCommand.Execute(product);
        _viewModel.AddToCartCommand.Execute(product);

        Assert.That(
            _viewModel.CartItems.Single().Quantity,
            Is.EqualTo(2));

        Assert.That(
            _viewModel.Total,
            Is.EqualTo(200m));
    }

    [Test]
    public void ClearCartCommand_ClearsCart()
    {
        var product =
            _viewModel.ProductsViewModel.Products.Single();

        _viewModel.AddToCartCommand.Execute(product);

        _viewModel.ClearCartCommand.Execute(null);

        Assert.That(
            _viewModel.HasItems,
            Is.False);

        Assert.That(
            _viewModel.Total,
            Is.EqualTo(0m));
    }

    [Test]
    public void Checkout_WithoutCustomer_CannotExecute()
    {
        var product =
            _viewModel.ProductsViewModel.Products.Single();

        _viewModel.AddToCartCommand.Execute(product);

        Assert.That(
            _viewModel.CanCheckout,
            Is.False);
    }

    [Test]
    public void Checkout_WithCustomerAndCart_CreatesOrder()
    {
        var product =
            _viewModel.ProductsViewModel.Products.Single();

        _viewModel.AddToCartCommand.Execute(product);

        _viewModel.CustomerViewModel.MobileNumber =
            "9876543210";

        _viewModel.CustomerViewModel
            .FindCustomerCommand
            .Execute(null);

        Assert.That(
            _viewModel.CanCheckout,
            Is.True);

        _viewModel.CheckoutCommand.Execute(null);

        Assert.That(
            _orderRepository.SavedOrders,
            Has.Count.EqualTo(1));

        var order =
            _orderRepository.SavedOrders.Single();

        Assert.That(
            order.CustomerId,
            Is.EqualTo(1));

        Assert.That(
            order.Items,
            Has.Count.EqualTo(1));

        Assert.That(
            order.Items.Single().Fruit.Name,
            Is.EqualTo("Apple"));
    }

    [Test]
    public void Checkout_AfterCompletion_ClearsCart()
    {
        var product =
            _viewModel.ProductsViewModel.Products.Single();

        _viewModel.AddToCartCommand.Execute(product);

        _viewModel.CustomerViewModel.MobileNumber =
            "9876543210";

        _viewModel.CustomerViewModel
            .FindCustomerCommand
            .Execute(null);

        _viewModel.CheckoutCommand.Execute(null);

        Assert.That(
            _viewModel.HasItems,
            Is.False);

        Assert.That(
            _viewModel.Total,
            Is.EqualTo(0m));

        Assert.That(
            _viewModel.CustomerViewModel.Customer,
            Is.Null);
    }

    [Test]
    public void NewOrder_ResetsCustomerAndCart()
    {
        var product =
            _viewModel.ProductsViewModel.Products.Single();

        _viewModel.AddToCartCommand.Execute(product);

        _viewModel.CustomerViewModel.MobileNumber =
            "9876543210";

        _viewModel.CustomerViewModel
            .FindCustomerCommand
            .Execute(null);

        _viewModel.NewOrderCommand.Execute(null);

        Assert.That(
            _viewModel.HasItems,
            Is.False);

        Assert.That(
            _viewModel.CustomerViewModel.Customer,
            Is.Null);

        Assert.That(
            _viewModel.CustomerViewModel.MobileNumber,
            Is.Empty);
    }
}