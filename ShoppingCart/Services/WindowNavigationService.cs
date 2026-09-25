using Microsoft.Extensions.DependencyInjection;

namespace ShoppingCart.WPF.Services;

/// <summary>
/// Handles navigation between application windows.
/// </summary>
public sealed class WindowNavigationService : IWindowNavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public WindowNavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public void OpenProductsWindow()
    {
        var window =
            _serviceProvider.GetRequiredService<ProductsWindow>();

        window.Owner = System.Windows.Application.Current.MainWindow;

        window.ShowDialog();
    }
}