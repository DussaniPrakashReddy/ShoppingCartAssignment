using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.WPF.Commands;
using ShoppingCart.WPF.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ShoppingCart.WPF.ViewModels;

/// <summary>
/// Root ViewModel for the Point of Sale screen.
/// </summary>
/// <remarks>
/// Coordinates customer, product and cart ViewModels while keeping
/// transaction and order responsibilities at the root level.
/// </remarks>
public sealed class ShoppingCartViewModel : INotifyPropertyChanged
{
    private readonly IOrderService _orderService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IWindowNavigationService _windowNavigationService;

    private int _orderId;
    private string _statusMessage = string.Empty;

    public ShoppingCartViewModel(
        CustomerViewModel customerViewModel,
        ProductsViewModel productsViewModel,
        IOrderService orderService,
        IShoppingCartService shoppingCartService,
        IWindowNavigationService windowNavigationService)
    {
        CustomerViewModel = customerViewModel
            ?? throw new ArgumentNullException(nameof(customerViewModel));

        ProductsViewModel = productsViewModel
            ?? throw new ArgumentNullException(nameof(productsViewModel));

        _orderService = orderService
            ?? throw new ArgumentNullException(nameof(orderService));

        _shoppingCartService = shoppingCartService
            ?? throw new ArgumentNullException(nameof(shoppingCartService));

        _windowNavigationService = windowNavigationService
            ?? throw new ArgumentNullException(nameof(windowNavigationService));

        CustomerViewModel.PropertyChanged +=
            CustomerViewModel_PropertyChanged;

        AddToCartCommand = new RelayCommand(
            parameter => AddToCart(parameter as IFruit),
            parameter => parameter is IFruit);

        IncreaseQuantityCommand = new RelayCommand(
            parameter => IncreaseQuantity(parameter as IFruit),
            parameter => parameter is IFruit);

        DecreaseQuantityCommand = new RelayCommand(
            parameter => DecreaseQuantity(parameter as IFruit),
            parameter => parameter is IFruit);

        RemoveFromCartCommand = new RelayCommand(
            parameter => RemoveFromCart(parameter as IFruit),
            parameter => parameter is IFruit);

        ClearCartCommand = new RelayCommand(
            _ => ClearCart(),
            _ => HasItems);

        NewOrderCommand = new RelayCommand(
            _ => StartNewOrder());

        OpenProductsCommand = new RelayCommand(
            _ => OpenProducts());

        CheckoutCommand = new RelayCommand(
            _ => Checkout(),
            _ => CanCheckout);
    }

    // ============================================================
    // CHILD VIEWMODELS
    // ============================================================

    /// <summary>
    /// Gets the ViewModel used by CustomerView.
    /// </summary>
    public CustomerViewModel CustomerViewModel { get; }

    /// <summary>
    /// Gets the ViewModel used by ProductsView.
    /// </summary>
    public ProductsViewModel ProductsViewModel { get; }

    // ============================================================
    // CART
    // ============================================================

    public IReadOnlyCollection<CartItem> CartItems =>
        _shoppingCartService.Items;

    public bool HasItems =>
        CartItems.Count > 0;

    public decimal Total =>
        _shoppingCartService.Total;

    // ============================================================
    // ORDER
    // ============================================================

    public int OrderId =>
        _orderId;

    public string StatusMessage
    {
        get => _statusMessage;

        private set
        {
            if (_statusMessage == value)
            {
                return;
            }

            _statusMessage = value;

            OnPropertyChanged();
        }
    }

    public bool CanCheckout =>
        CustomerViewModel.Customer is not null
        && HasItems;

    // ============================================================
    // COMMANDS
    // ============================================================

    public ICommand AddToCartCommand { get; }

    public ICommand IncreaseQuantityCommand { get; }

    public ICommand DecreaseQuantityCommand { get; }

    public ICommand RemoveFromCartCommand { get; }

    public ICommand ClearCartCommand { get; }

    public ICommand NewOrderCommand { get; }

    public ICommand OpenProductsCommand { get; }

    public ICommand CheckoutCommand { get; }

    // ============================================================
    // PRODUCT NAVIGATION
    // ============================================================

    private void OpenProducts()
    {
        _windowNavigationService.OpenProductsWindow();

        // ProductsWindow is modal. Once it closes,
        // reload the POS product catalog.
        ProductsViewModel.Refresh();
    }

    // ============================================================
    // CART OPERATIONS
    // ============================================================

    private void AddToCart(IFruit? fruit)
    {
        if (fruit is null)
        {
            return;
        }

        _shoppingCartService.Add(fruit);

        RefreshCart();
    }

    private void IncreaseQuantity(IFruit? fruit)
    {
        if (fruit is null)
        {
            return;
        }

        _shoppingCartService.Increase(fruit);

        RefreshCart();
    }

    private void DecreaseQuantity(IFruit? fruit)
    {
        if (fruit is null)
        {
            return;
        }

        _shoppingCartService.Decrease(fruit);

        RefreshCart();
    }

    private void RemoveFromCart(IFruit? fruit)
    {
        if (fruit is null)
        {
            return;
        }

        _shoppingCartService.Remove(fruit);

        RefreshCart();
    }

    private void ClearCart()
    {
        _shoppingCartService.Clear();

        RefreshCart();
    }

    // ============================================================
    // NEW ORDER
    // ============================================================

    private void StartNewOrder()
    {
        _shoppingCartService.Clear();

        CustomerViewModel.Reset();

        _orderId = 0;

        StatusMessage =
            "Ready for a new order.";

        OnPropertyChanged(nameof(CartItems));
        OnPropertyChanged(nameof(HasItems));
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(OrderId));
        OnPropertyChanged(nameof(CanCheckout));

        RaiseCommandStates();
    }

    // ============================================================
    // CHECKOUT
    // ============================================================

    private void Checkout()
    {
        if (!CanCheckout ||
            CustomerViewModel.Customer is null)
        {
            return;
        }

        var orderId =
            _orderService.GetNextOrderId();

        var order = new Order
        {
            OrderId = orderId,
            CustomerId = CustomerViewModel.Customer.Id,
            CreatedAt = DateTime.Now,
            Items = CartItems.ToList()
        };

        _orderService.PlaceOrder(order);

        _orderId = orderId;

        StatusMessage =
            $"Order #{orderId} completed successfully.";

        _shoppingCartService.Clear();

        CustomerViewModel.Reset();

        OnPropertyChanged(nameof(OrderId));
        OnPropertyChanged(nameof(CartItems));
        OnPropertyChanged(nameof(HasItems));
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(CanCheckout));

        RaiseCommandStates();
    }

    // ============================================================
    // CUSTOMER VIEWMODEL CHANGES
    // ============================================================

    private void CustomerViewModel_PropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CustomerViewModel.Customer))
        {
            OnPropertyChanged(nameof(CanCheckout));

            RaiseCommandStates();
        }
    }

    // ============================================================
    // REFRESH
    // ============================================================

    private void RefreshCart()
    {
        OnPropertyChanged(nameof(CartItems));
        OnPropertyChanged(nameof(HasItems));
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(CanCheckout));

        RaiseCommandStates();
    }

    private void RaiseCommandStates()
    {
        if (ClearCartCommand is RelayCommand clearCartCommand)
        {
            clearCartCommand.RaiseCanExecuteChanged();
        }

        if (CheckoutCommand is RelayCommand checkoutCommand)
        {
            checkoutCommand.RaiseCanExecuteChanged();
        }
    }

    // ============================================================
    // PROPERTY CHANGE
    // ============================================================

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}