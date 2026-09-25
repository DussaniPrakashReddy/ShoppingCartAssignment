using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.WPF.Commands;

namespace ShoppingCart.WPF.ViewModels;

/// <summary>
/// ViewModel responsible for product catalog administration.
/// </summary>
/// <remarks>
/// Provides functionality to add, edit, delete, search and manage
/// product availability.
/// </remarks>
public sealed class ProductManagementViewModel : INotifyPropertyChanged
{
    private readonly IProductService _productService;

    private List<FruitProduct> _allProducts = new();

    private FruitProduct? _selectedProduct;

    private string _searchText = string.Empty;
    private string _productName = string.Empty;
    private string _category = "Fruits";
    private decimal _price;
    private string _emoji = string.Empty;
    private bool _isAvailable = true;
    private bool _isEditMode;
    private string _statusMessage = string.Empty;

    public ProductManagementViewModel(
        IProductService productService)
    {
        _productService = productService
            ?? throw new ArgumentNullException(nameof(productService));

        Products = new ObservableCollection<FruitProduct>();

        LoadProductsCommand = new RelayCommand(
            _ => LoadProducts());

        AddProductCommand = new RelayCommand(
            _ => StartAddProduct());

        EditProductCommand = new RelayCommand(
            parameter => EditProduct(parameter as FruitProduct),
            parameter => parameter is FruitProduct);

        DeleteProductCommand = new RelayCommand(
            parameter => DeleteProduct(parameter as FruitProduct),
            parameter => parameter is FruitProduct);

        SaveProductCommand = new RelayCommand(
            _ => SaveProduct(),
            _ => CanSaveProduct);

        CancelCommand = new RelayCommand(
            _ => CancelEdit(),
            _ => IsEditMode);

        ToggleAvailabilityCommand = new RelayCommand(
            parameter => ToggleAvailability(parameter as FruitProduct),
            parameter => parameter is FruitProduct);

        LoadProducts();
    }

    // ============================================================
    // COLLECTION
    // ============================================================

    public ObservableCollection<FruitProduct> Products { get; }

    public FruitProduct? SelectedProduct
    {
        get => _selectedProduct;

        set
        {
            if (ReferenceEquals(_selectedProduct, value))
            {
                return;
            }

            _selectedProduct = value;

            OnPropertyChanged();

            RaiseCommandStates();
        }
    }

    // ============================================================
    // SEARCH
    // ============================================================

    public string SearchText
    {
        get => _searchText;

        set
        {
            if (_searchText == value)
            {
                return;
            }

            _searchText = value;

            OnPropertyChanged();

            ApplyFilter();
        }
    }

    // ============================================================
    // PRODUCT FORM
    // ============================================================

    public string ProductName
    {
        get => _productName;

        set
        {
            if (_productName == value)
            {
                return;
            }

            _productName = value;

            OnPropertyChanged();

            RaiseCommandStates();
        }
    }

    public string Category
    {
        get => _category;

        set
        {
            if (_category == value)
            {
                return;
            }

            _category = value;

            OnPropertyChanged();

            RaiseCommandStates();
        }
    }

    public decimal Price
    {
        get => _price;

        set
        {
            if (_price == value)
            {
                return;
            }

            _price = value;

            OnPropertyChanged();

            RaiseCommandStates();
        }
    }

    public string Emoji
    {
        get => _emoji;

        set
        {
            if (_emoji == value)
            {
                return;
            }

            _emoji = value;

            OnPropertyChanged();
        }
    }

    public bool IsAvailable
    {
        get => _isAvailable;

        set
        {
            if (_isAvailable == value)
            {
                return;
            }

            _isAvailable = value;

            OnPropertyChanged();
        }
    }

    public bool IsEditMode
    {
        get => _isEditMode;

        private set
        {
            if (_isEditMode == value)
            {
                return;
            }

            _isEditMode = value;

            OnPropertyChanged();

            RaiseCommandStates();
        }
    }

    public bool CanSaveProduct =>
        !string.IsNullOrWhiteSpace(ProductName)
        && !string.IsNullOrWhiteSpace(Category)
        && Price >= 0;

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

    // ============================================================
    // COMMANDS
    // ============================================================

    public ICommand LoadProductsCommand { get; }

    public ICommand AddProductCommand { get; }

    public ICommand EditProductCommand { get; }

    public ICommand DeleteProductCommand { get; }

    public ICommand SaveProductCommand { get; }

    public ICommand CancelCommand { get; }

    public ICommand ToggleAvailabilityCommand { get; }

    // ============================================================
    // LOAD
    // ============================================================

    private void LoadProducts()
    {
        try
        {
            var products =
                _productService
                    .GetProducts()
                    .ToList();

            _allProducts = products;

            ApplyFilter();

            StatusMessage =
                $"{products.Count} product(s) loaded.";
        }
        catch (Exception exception)
        {
            StatusMessage =
                $"Unable to load products: {exception.Message}";
        }
    }

    // ============================================================
    // ADD
    // ============================================================

    private void StartAddProduct()
    {
        SelectedProduct = null;

        ProductName = string.Empty;
        Category = "Fruits";
        Price = 0m;
        Emoji = string.Empty;
        IsAvailable = true;

        IsEditMode = true;

        StatusMessage =
            "Enter the new product details.";
    }

    // ============================================================
    // EDIT
    // ============================================================

    private void EditProduct(FruitProduct? product)
    {
        if (product is null)
        {
            return;
        }

        SelectedProduct = product;

        ProductName = product.Name;
        Category = product.Category;
        Price = product.Price;
        Emoji = product.Emoji;
        IsAvailable = product.IsAvailable;

        IsEditMode = true;

        StatusMessage =
            $"Editing {product.Name}.";
    }

    // ============================================================
    // SAVE
    // ============================================================

    private void SaveProduct()
    {
        if (!CanSaveProduct)
        {
            StatusMessage =
                "Please provide a valid product name, category and price.";

            return;
        }

        try
        {
            if (SelectedProduct is null)
            {
                var product =
                    _productService.CreateProduct(
                        ProductName,
                        Category,
                        Price,
                        Emoji);

                product.IsAvailable = IsAvailable;

                if (!IsAvailable)
                {
                    _productService.UpdateProduct(product);
                }

                StatusMessage =
                    $"{product.Name} added successfully.";
            }
            else
            {
                SelectedProduct.Name = ProductName.Trim();
                SelectedProduct.Category = Category.Trim();
                SelectedProduct.Price = Price;
                SelectedProduct.Emoji = Emoji.Trim();
                SelectedProduct.IsAvailable = IsAvailable;

                _productService.UpdateProduct(
                    SelectedProduct);

                StatusMessage =
                    $"{SelectedProduct.Name} updated successfully.";
            }

            LoadProducts();

            ClearForm();

            IsEditMode = false;
        }
        catch (Exception exception)
        {
            StatusMessage =
                $"Unable to save product: {exception.Message}";
        }
    }

    // ============================================================
    // DELETE
    // ============================================================

    private void DeleteProduct(FruitProduct? product)
    {
        if (product is null)
        {
            return;
        }

        try
        {
            _productService.DeleteProduct(product.Id);

            StatusMessage =
                $"{product.Name} deleted successfully.";

            if (ReferenceEquals(SelectedProduct, product))
            {
                ClearForm();
                IsEditMode = false;
            }

            LoadProducts();
        }
        catch (Exception exception)
        {
            StatusMessage =
                $"Unable to delete product: {exception.Message}";
        }
    }

    // ============================================================
    // AVAILABILITY
    // ============================================================

    private void ToggleAvailability(FruitProduct? product)
    {
        if (product is null)
        {
            return;
        }

        try
        {
            product.IsAvailable =
                !product.IsAvailable;

            _productService.UpdateProduct(product);

            StatusMessage =
                product.IsAvailable
                    ? $"{product.Name} is now available."
                    : $"{product.Name} is now unavailable.";

            ApplyFilter();
        }
        catch (Exception exception)
        {
            product.IsAvailable =
                !product.IsAvailable;

            StatusMessage =
                $"Unable to update availability: {exception.Message}";
        }
    }

    // ============================================================
    // CANCEL
    // ============================================================

    private void CancelEdit()
    {
        ClearForm();

        IsEditMode = false;

        StatusMessage =
            "Product editing cancelled.";
    }

    private void ClearForm()
    {
        SelectedProduct = null;

        ProductName = string.Empty;
        Category = "Fruits";
        Price = 0m;
        Emoji = string.Empty;
        IsAvailable = true;
    }

    // ============================================================
    // SEARCH
    // ============================================================

    private void ApplyFilter()
    {
        var search =
            SearchText.Trim();

        var filteredProducts =
            string.IsNullOrWhiteSpace(search)
                ? _allProducts
                : _allProducts
                    .Where(product =>
                        product.Name.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        product.Category.Contains(
                            search,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();

        Products.Clear();

        foreach (var product in filteredProducts)
        {
            Products.Add(product);
        }
    }

    // ============================================================
    // COMMAND STATE
    // ============================================================

    private void RaiseCommandStates()
    {
        if (SaveProductCommand is RelayCommand saveCommand)
        {
            saveCommand.RaiseCanExecuteChanged();
        }

        if (CancelCommand is RelayCommand cancelCommand)
        {
            cancelCommand.RaiseCanExecuteChanged();
        }

        if (EditProductCommand is RelayCommand editCommand)
        {
            editCommand.RaiseCanExecuteChanged();
        }

        if (DeleteProductCommand is RelayCommand deleteCommand)
        {
            deleteCommand.RaiseCanExecuteChanged();
        }

        if (ToggleAvailabilityCommand is RelayCommand availabilityCommand)
        {
            availabilityCommand.RaiseCanExecuteChanged();
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