using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Domain.Entities;

/// <summary>
/// Represents a fruit selected by the customer together
/// with the requested quantity.
/// </summary>
public sealed class CartItem : INotifyPropertyChanged
{
    private int _quantity;


    /// <summary>
    /// Gets or initializes the fruit associated with this cart item.
    /// </summary>
    public required IFruit Fruit { get; init; }


    /// <summary>
    /// Gets or sets the quantity of the fruit in the cart.
    /// </summary>
    public int Quantity
    {
        get => _quantity;

        set
        {
            if (_quantity == value)
            {
                return;
            }

            _quantity = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(LineTotal));
        }
    }


    /// <summary>
    /// Gets the total price for this cart item.
    /// </summary>
    public decimal LineTotal =>
        Fruit.Price * Quantity;


    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">
    /// Name of the property that changed.
    /// </param>
    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }


    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
}