using ShoppingCart.WPF.ViewModels;
using System.Windows;

namespace ShoppingCart.WPF;

/// <summary>
/// Represents the main shopping cart window.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    /// <param name="viewModel">
    /// ViewModel supplied through dependency injection.
    /// </param>
    public MainWindow(ShoppingCartViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}