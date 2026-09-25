using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShoppingCart.WPF.Views;

/// <summary>
/// Displays products available for purchase in the POS screen.
/// </summary>
public partial class ProductsView : UserControl
{
    /// <summary>
    /// Identifies the AddToCartCommand dependency property.
    /// </summary>
    public static readonly DependencyProperty AddToCartCommandProperty =
        DependencyProperty.Register(
            nameof(AddToCartCommand),
            typeof(ICommand),
            typeof(ProductsView),
            new PropertyMetadata(null));

    public ProductsView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the command used to add a product to the cart.
    /// </summary>
    public ICommand? AddToCartCommand
    {
        get => (ICommand?)GetValue(AddToCartCommandProperty);
        set => SetValue(AddToCartCommandProperty, value);
    }
}