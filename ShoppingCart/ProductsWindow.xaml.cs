using System.Windows;
using ShoppingCart.WPF.ViewModels;

namespace ShoppingCart.WPF;

/// <summary>
/// Provides the user interface for managing products.
/// </summary>
public partial class ProductsWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ProductsWindow"/> class.
    /// </summary>
    public ProductsWindow(
        ProductManagementViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}