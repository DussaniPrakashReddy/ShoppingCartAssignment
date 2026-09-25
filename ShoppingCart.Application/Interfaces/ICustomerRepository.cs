using ShoppingCart.Domain.Entities;

public interface ICustomerRepository
{
    Customer? FindByMobileNumber(string mobileNumber);

    int GetNextCustomerId();

    Customer Add(Customer customer);
}