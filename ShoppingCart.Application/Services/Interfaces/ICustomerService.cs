using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Application.Services.Interfaces;

/// <summary>
/// Provides customer-related application operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Finds a customer using their mobile number.
    /// </summary>
    Customer? FindCustomer(string mobileNumber);

    Customer CreateCustomer(string name, string mobileNumber);
}