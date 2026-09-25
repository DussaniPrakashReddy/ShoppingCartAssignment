using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Repositories;

/// <summary>
/// Provides customer information using in-memory data.
/// </summary>
public sealed class CustomerRepository : ICustomerRepository
{
    private int _lastCustomerId = 2;
    private readonly List<Customer> _customers =
    [
        new Customer
        {
            Id = 1,
            Name = "Prakash",
            MobileNumber = "9876543210"
        },
        new Customer
        {
            Id = 2,
            Name = "Ravi",
            MobileNumber = "9123456789"
        }
    ];

    /// <inheritdoc />
    public Customer Add(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        _customers.Add(customer);

        return customer;
    }

    /// <inheritdoc />
    public Customer? FindByMobileNumber(string mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            return null;
        }

        return _customers.FirstOrDefault(
            customer => customer.MobileNumber == mobileNumber.Trim());
    }
    public int GetNextCustomerId()
    {
        return ++_lastCustomerId;
    }
}