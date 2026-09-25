using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Entities;
using ShoppingCart.WPF.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ShoppingCart.WPF.ViewModels;

/// <summary>
/// ViewModel responsible for customer lookup and customer creation.
/// </summary>
public sealed class CustomerViewModel : INotifyPropertyChanged
{
    private readonly ICustomerService _customerService;

    private string _mobileNumber = string.Empty;
    private Customer? _customer;
    private string _statusMessage = string.Empty;
    private bool _isCustomerSearchInProgress;
    private bool _isCreateCustomerVisible;
    private string _newCustomerName = string.Empty;

    public CustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService
            ?? throw new ArgumentNullException(nameof(customerService));

        FindCustomerCommand = new RelayCommand(
            _ => FindCustomer(),
            _ => !string.IsNullOrWhiteSpace(MobileNumber)
                 && !IsCustomerSearchInProgress);

        CreateCustomerCommand = new RelayCommand(
            _ => CreateCustomer(),
            _ => !string.IsNullOrWhiteSpace(NewCustomerName)
                 && !string.IsNullOrWhiteSpace(MobileNumber));
    }

    // ============================================================
    // PROPERTIES
    // ============================================================

    public string MobileNumber
    {
        get => _mobileNumber;

        set
        {
            if (_mobileNumber == value)
            {
                return;
            }

            _mobileNumber = value;

            OnPropertyChanged();

            ClearCustomerIfMobileNumberChanged();

            RaiseCommandStates();
        }
    }

    public Customer? Customer =>
        _customer;

    public string CustomerDisplayName =>
        _customer is null
            ? "No customer selected"
            : _customer.Name;

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

    public bool IsCustomerSearchInProgress
    {
        get => _isCustomerSearchInProgress;

        private set
        {
            if (_isCustomerSearchInProgress == value)
            {
                return;
            }

            _isCustomerSearchInProgress = value;

            OnPropertyChanged();

            RaiseCommandStates();
        }
    }

    public bool IsCreateCustomerVisible
    {
        get => _isCreateCustomerVisible;

        private set
        {
            if (_isCreateCustomerVisible == value)
            {
                return;
            }

            _isCreateCustomerVisible = value;

            OnPropertyChanged();
        }
    }

    public string NewCustomerName
    {
        get => _newCustomerName;

        set
        {
            if (_newCustomerName == value)
            {
                return;
            }

            _newCustomerName = value;

            OnPropertyChanged();

            RaiseCommandStates();
        }
    }

    // ============================================================
    // COMMANDS
    // ============================================================

    public ICommand FindCustomerCommand { get; }

    public ICommand CreateCustomerCommand { get; }

    // ============================================================
    // CUSTOMER SEARCH
    // ============================================================

    private void FindCustomer()
    {
        var mobileNumber = MobileNumber.Trim();

        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            StatusMessage = "Please enter a mobile number.";
            return;
        }

        try
        {
            IsCustomerSearchInProgress = true;

            StatusMessage = "Searching customer...";

            var customer =
                _customerService.FindCustomer(mobileNumber);

            if (customer is null)
            {
                _customer = null;

                IsCreateCustomerVisible = true;

                StatusMessage =
                    "Customer not found. You can create a new customer.";

                OnPropertyChanged(nameof(Customer));
                OnPropertyChanged(nameof(CustomerDisplayName));

                return;
            }

            _customer = customer;

            IsCreateCustomerVisible = false;
            _newCustomerName = string.Empty;

            StatusMessage =
                $"Customer found: {customer.Name}";

            OnPropertyChanged(nameof(Customer));
            OnPropertyChanged(nameof(CustomerDisplayName));
            OnPropertyChanged(nameof(NewCustomerName));
        }
        finally
        {
            IsCustomerSearchInProgress = false;
        }
    }

    // ============================================================
    // CREATE CUSTOMER
    // ============================================================

    private void CreateCustomer()
    {
        var name = NewCustomerName.Trim();
        var mobileNumber = MobileNumber.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            StatusMessage = "Please enter the customer name.";
            return;
        }

        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            StatusMessage = "Please enter the customer mobile number.";
            return;
        }

        try
        {
            var customer =
                _customerService.CreateCustomer(
                    name,
                    mobileNumber);

            _customer = customer;
            _newCustomerName = string.Empty;

            IsCreateCustomerVisible = false;

            StatusMessage =
                $"Customer created: {customer.Name}";

            OnPropertyChanged(nameof(Customer));
            OnPropertyChanged(nameof(CustomerDisplayName));
            OnPropertyChanged(nameof(NewCustomerName));
        }
        catch (InvalidOperationException exception)
        {
            StatusMessage = exception.Message;
        }
        catch (ArgumentException exception)
        {
            StatusMessage = exception.Message;
        }
    }

    // ============================================================
    // RESET
    // ============================================================

    /// <summary>
    /// Clears the current customer selection and customer input.
    /// </summary>
    public void Reset()
    {
        _customer = null;
        _mobileNumber = string.Empty;
        _newCustomerName = string.Empty;
        _statusMessage = string.Empty;
        _isCustomerSearchInProgress = false;
        _isCreateCustomerVisible = false;

        OnPropertyChanged(nameof(Customer));
        OnPropertyChanged(nameof(CustomerDisplayName));
        OnPropertyChanged(nameof(MobileNumber));
        OnPropertyChanged(nameof(NewCustomerName));
        OnPropertyChanged(nameof(StatusMessage));
        OnPropertyChanged(nameof(IsCustomerSearchInProgress));
        OnPropertyChanged(nameof(IsCreateCustomerVisible));

        RaiseCommandStates();
    }

    // ============================================================
    // HELPERS
    // ============================================================

    private void ClearCustomerIfMobileNumberChanged()
    {
        if (_customer is null)
        {
            return;
        }

        _customer = null;

        IsCreateCustomerVisible = false;
        _newCustomerName = string.Empty;

        StatusMessage = string.Empty;

        OnPropertyChanged(nameof(Customer));
        OnPropertyChanged(nameof(CustomerDisplayName));
        OnPropertyChanged(nameof(NewCustomerName));
    }

    private void RaiseCommandStates()
    {
        if (FindCustomerCommand is RelayCommand findCommand)
        {
            findCommand.RaiseCanExecuteChanged();
        }

        if (CreateCustomerCommand is RelayCommand createCommand)
        {
            createCommand.RaiseCanExecuteChanged();
        }
    }

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}