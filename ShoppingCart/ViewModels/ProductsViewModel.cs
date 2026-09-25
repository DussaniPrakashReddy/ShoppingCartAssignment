using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Interfaces;
using System.Collections.ObjectModel;

namespace ShoppingCart.WPF.ViewModels;

/// <summary>
/// ViewModel for the products displayed on the Point of Sale screen.
/// </summary>
/// <remarks>
/// This ViewModel is responsible only for loading products that are
/// available for sale. Cart operations remain owned by the root
/// ShoppingCartViewModel.
/// </remarks>
public sealed class ProductsViewModel
{
    private readonly IProductService _productService;

    public ProductsViewModel(IProductService productService)
    {
        _productService = productService
            ?? throw new ArgumentNullException(nameof(productService));

        Products = new ObservableCollection<IFruit>();

        Refresh();
    }

    /// <summary>
    /// Gets the products available for sale.
    /// </summary>
    public ObservableCollection<IFruit> Products { get; }

    /// <summary>
    /// Reloads available products from the product service.
    /// </summary>
    public void Refresh()
    {
        Products.Clear();

        foreach (var product in _productService.GetAvailableProducts())
        {
            Products.Add(product);
        }
    }
}