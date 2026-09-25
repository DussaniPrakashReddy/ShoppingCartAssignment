namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents a customer who can place an order.
/// </summary>
public sealed class Customer
{
    /// <summary>
    /// Gets the unique customer identifier.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the customer's name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the customer's mobile number.
    /// </summary>
    public string MobileNumber { get; init; } = string.Empty;
}