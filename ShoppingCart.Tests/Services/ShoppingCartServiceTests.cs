using NUnit.Framework;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Tests.Services;

[TestFixture]
public sealed class ShoppingCartServiceTests
{
    private ShoppingCartService _service = null!;
    private IFruit _apple = null!;
    private IFruit _mango = null!;

    [SetUp]
    public void SetUp()
    {
        _service = new ShoppingCartService();

        _apple = new TestFruit
        {
            Id = 101,
            Name = "Apple",
            Price = 100m,
            Emoji = "🍎"
        };

        _mango = new TestFruit
        {
            Id = 102,
            Name = "Mango",
            Price = 150m,
            Emoji = "🥭"
        };
    }

    [Test]
    public void Add_NewProduct_AddsProductToCart()
    {
        _service.Add(_apple);

        Assert.That(
            _service.Items,
            Has.Count.EqualTo(1));

        Assert.That(
            _service.Items.Single().Quantity,
            Is.EqualTo(1));
    }

    [Test]
    public void Add_SameProductTwice_IncreasesQuantity()
    {
        _service.Add(_apple);
        _service.Add(_apple);

        Assert.That(
            _service.Items,
            Has.Count.EqualTo(1));

        Assert.That(
            _service.Items.Single().Quantity,
            Is.EqualTo(2));
    }

    [Test]
    public void Add_DifferentProducts_AddsSeparateItems()
    {
        _service.Add(_apple);
        _service.Add(_mango);

        Assert.That(
            _service.Items,
            Has.Count.EqualTo(2));
    }

    [Test]
    public void Add_CalculatesTotal()
    {
        _service.Add(_apple);
        _service.Add(_mango);

        Assert.That(
            _service.Total,
            Is.EqualTo(250m));
    }

    [Test]
    public void Increase_IncreasesQuantity()
    {
        _service.Add(_apple);

        _service.Increase(_apple);

        Assert.That(
            _service.Items.Single().Quantity,
            Is.EqualTo(2));
    }

    [Test]
    public void Increase_UpdatesTotal()
    {
        _service.Add(_apple);

        _service.Increase(_apple);

        Assert.That(
            _service.Total,
            Is.EqualTo(200m));
    }

    [Test]
    public void Decrease_WhenQuantityIsGreaterThanOne_DecreasesQuantity()
    {
        _service.Add(_apple);
        _service.Increase(_apple);

        _service.Decrease(_apple);

        Assert.That(
            _service.Items.Single().Quantity,
            Is.EqualTo(1));
    }

    [Test]
    public void Decrease_WhenQuantityIsOne_RemovesItem()
    {
        _service.Add(_apple);

        _service.Decrease(_apple);

        Assert.That(
            _service.Items,
            Is.Empty);
    }

    [Test]
    public void Remove_RemovesProduct()
    {
        _service.Add(_apple);

        _service.Remove(_apple);

        Assert.That(
            _service.Items,
            Is.Empty);
    }

    [Test]
    public void Clear_RemovesAllProducts()
    {
        _service.Add(_apple);
        _service.Add(_mango);

        _service.Clear();

        Assert.That(
            _service.Items,
            Is.Empty);

        Assert.That(
            _service.Total,
            Is.EqualTo(0m));
    }

    [Test]
    public void Add_NullProduct_ThrowsArgumentNullException()
    {
        Assert.That(
            () => _service.Add(null!),
            Throws.TypeOf<ArgumentNullException>());
    }

    private sealed class TestFruit : IFruit
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public decimal Price { get; init; }

        public string Emoji { get; init; } = string.Empty;
    }
}