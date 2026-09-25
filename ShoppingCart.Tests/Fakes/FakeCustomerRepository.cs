using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Tests.Fakes;

/// <summary>
/// In-memory customer repository used for unit testing.
/// </summary>
public sealed class FakeCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();

    private int _lastCustomerId;

    public Customer? FindByMobileNumber(string mobileNumber)
    {
        return _customers.FirstOrDefault(
            customer => customer.MobileNumber == mobileNumber);
    }

    public Customer Add(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        _customers.Add(customer);

        if (customer.Id > _lastCustomerId)
        {
            _lastCustomerId = customer.Id;
        }

        return customer;
    }

    public int GetNextCustomerId()
    {
        return ++_lastCustomerId;
    }

    public void Seed(params Customer[] customers)
    {
        foreach (var customer in customers)
        {
            Add(customer);
        }
    }
}