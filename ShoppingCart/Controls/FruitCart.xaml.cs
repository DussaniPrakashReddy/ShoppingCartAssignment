using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.WPF.Controls;

/// <summary>
/// Displays a fruit and provides an action for adding the fruit
/// to the shopping cart.
/// </summary>
public partial class FruitCart : UserControl
{
    /// <summary>
    /// Identifies the <see cref="Fruit"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FruitProperty =
        DependencyProperty.Register(
            nameof(Fruit),
            typeof(IFruit),
            typeof(FruitCart),
            new PropertyMetadata(null));

    /// <summary>
    /// Identifies the <see cref="FruitName"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FruitNameProperty =
        DependencyProperty.Register(
            nameof(FruitName),
            typeof(string),
            typeof(FruitCart),
            new PropertyMetadata(string.Empty));


    /// <summary>
    /// Identifies the <see cref="Price"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty PriceProperty =
        DependencyProperty.Register(
            nameof(Price),
            typeof(decimal),
            typeof(FruitCart),
            new PropertyMetadata(0m));

    /// <summary>
    /// Identifies the <see cref="AddCommand"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AddCommandProperty =
        DependencyProperty.Register(
            nameof(AddCommand),
            typeof(ICommand),
            typeof(FruitCart),
            new PropertyMetadata(null));

    /// <summary>
    /// Initializes a new instance of the <see cref="FruitCart"/> class.
    /// </summary>
    public FruitCart()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the fruit represented by this card.
    /// </summary>
    public IFruit? Fruit
    {
        get => (IFruit?)GetValue(FruitProperty);
        set => SetValue(FruitProperty, value);
    }

    /// <summary>
    /// Gets or sets the display name of the fruit.
    /// </summary>
    public string FruitName
    {
        get => (string)GetValue(FruitNameProperty);
        set => SetValue(FruitNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the price of the fruit.
    /// </summary>
    public decimal Price
    {
        get => (decimal)GetValue(PriceProperty);
        set => SetValue(PriceProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the user adds
    /// the fruit to the cart.
    /// </summary>
    public ICommand? AddCommand
    {
        get => (ICommand?)GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }
}