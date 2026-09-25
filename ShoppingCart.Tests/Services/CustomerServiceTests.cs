using NUnit.Framework;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Tests.Fakes;

namespace ShoppingCart.Tests.Services;

[TestFixture]
public sealed class CustomerServiceTests
{
    private FakeCustomerRepository _repository = null!;
    private CustomerService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new FakeCustomerRepository();

        _service = new CustomerService(_repository);
    }

    [Test]
    public void FindCustomer_ExistingMobile_ReturnsCustomer()
    {
        _repository.Seed(
            new Customer
            {
                Id = 1,
                Name = "Prakash",
                MobileNumber = "9876543210"
            });

        var customer =
            _service.FindCustomer("9876543210");

        Assert.That(customer, Is.Not.Null);
        Assert.That(customer!.Id, Is.EqualTo(1));
        Assert.That(customer.Name, Is.EqualTo("Prakash"));
    }

    [Test]
    public void FindCustomer_UnknownMobile_ReturnsNull()
    {
        var customer =
            _service.FindCustomer("9999999999");

        Assert.That(customer, Is.Null);
    }

    [Test]
    public void FindCustomer_EmptyMobile_ReturnsNull()
    {
        var customer =
            _service.FindCustomer(string.Empty);

        Assert.That(customer, Is.Null);
    }

    [Test]
    public void FindCustomer_WhitespaceMobile_ReturnsNull()
    {
        var customer =
            _service.FindCustomer("   ");

        Assert.That(customer, Is.Null);
    }

    [Test]
    public void CreateCustomer_CreatesCustomer()
    {
        var customer =
            _service.CreateCustomer(
                "Ravi",
                "9999999999");

        Assert.That(customer, Is.Not.Null);
        Assert.That(customer.Id, Is.EqualTo(1));
        Assert.That(customer.Name, Is.EqualTo("Ravi"));
        Assert.That(
            customer.MobileNumber,
            Is.EqualTo("9999999999"));
    }

    [Test]
    public void CreateCustomer_TrimsValues()
    {
        var customer =
            _service.CreateCustomer(
                " Ravi ",
                " 9999999999 ");

        Assert.That(customer.Name, Is.EqualTo("Ravi"));
        Assert.That(
            customer.MobileNumber,
            Is.EqualTo("9999999999"));
    }

    [Test]
    public void CreateCustomer_EmptyName_Throws()
    {
        Assert.That(
            () => _service.CreateCustomer(
                string.Empty,
                "9999999999"),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void CreateCustomer_EmptyMobile_Throws()
    {
        Assert.That(
            () => _service.CreateCustomer(
                "Ravi",
                string.Empty),
            Throws.TypeOf<ArgumentException>());
    }
}