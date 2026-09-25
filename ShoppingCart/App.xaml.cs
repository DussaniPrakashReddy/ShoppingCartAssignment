using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Services;
using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Infrastructure.Repositories;
using ShoppingCart.WPF.Services;
using ShoppingCart.WPF.ViewModels;
using System.Windows;

namespace ShoppingCart.WPF;

/// <summary>
/// Represents the application entry point and dependency injection
/// composition root for the ShoppingCart application.
/// </summary>
public partial class App : System.Windows.Application
{
    private ServiceProvider? _serviceProvider;

    /// <summary>
    /// Gets the configured dependency injection service provider.
    /// </summary>
    public IServiceProvider Services =>
        _serviceProvider
        ?? throw new InvalidOperationException(
            "The service provider has not been initialized.");

    /// <inheritdoc />
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        ConfigureServices(services);

        _serviceProvider =
            services.BuildServiceProvider();

        var mainWindow =
            _serviceProvider.GetRequiredService<MainWindow>();

        MainWindow = mainWindow;

        mainWindow.Show();
    }

    private static void ConfigureServices(
        IServiceCollection services)
    {
        // ============================================================
        // REPOSITORIES
        // ============================================================

        services.AddSingleton<
            ICustomerRepository,
            XmlCustomerRepository>();

        services.AddSingleton<
            IOrderRepository,
            XmlOrderRepository>();

        services.AddSingleton<
            IProductRepository,
            XmlProductRepository>();


        // ============================================================
        // APPLICATION SERVICES
        // ============================================================

        services.AddSingleton<
            ICustomerService,
            CustomerService>();

        services.AddSingleton<
            IOrderService,
            OrderService>();

        services.AddSingleton<
            IProductService,
            ProductService>();

        services.AddSingleton<
            IShoppingCartService,
            ShoppingCartService>();


        // ============================================================
        // WPF SERVICES
        // ============================================================

        services.AddSingleton<
            IWindowNavigationService,
            WindowNavigationService>();


        // ============================================================
        // CHILD VIEWMODELS
        // ============================================================

        services.AddTransient<CustomerViewModel>();

        services.AddTransient<ProductsViewModel>();

        services.AddTransient<ProductManagementViewModel>();


        // ============================================================
        // WINDOWS
        // ============================================================

        services.AddTransient<ProductsWindow>();

        services.AddTransient<MainWindow>();


        // ============================================================
        // ROOT VIEWMODEL
        // ============================================================

        services.AddTransient<ShoppingCartViewModel>();
    }
}