using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Services;

/// <summary>
/// Implements customer-related application operations.
/// </summary>
public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="CustomerService"/>.
    /// </summary>
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository =
            customerRepository
            ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public Customer CreateCustomer(string name, string mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Customer name is required.",
                nameof(name));
        }

        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            throw new ArgumentException(
                "Customer mobile number is required.",
                nameof(mobileNumber));
        }

        var normalizedMobileNumber = mobileNumber.Trim();

        if (_customerRepository.FindByMobileNumber(normalizedMobileNumber)
            is not null)
        {
            throw new InvalidOperationException(
                "A customer with this mobile number already exists.");
        }

        var customer = new Customer
        {
            Id = _customerRepository.GetNextCustomerId(),
            Name = name.Trim(),
            MobileNumber = normalizedMobileNumber
        };

        return _customerRepository.Add(customer);
    }

    /// <inheritdoc />
    public Customer? FindCustomer(string mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            return null;
        }

        return _customerRepository.FindByMobileNumber(
            mobileNumber.Trim());
    }

    private int GenerateCustomerId()
    {
        // Temporary implementation while customer persistence
        // is still in-memory.
        var existingIds = new[]
        {
            _customerRepository.FindByMobileNumber("9876543210")?.Id ?? 0,
            _customerRepository.FindByMobileNumber("9123456789")?.Id ?? 0
        };

        return existingIds.Max() + 1;
    }
}