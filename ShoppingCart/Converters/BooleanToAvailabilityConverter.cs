using System.Globalization;
using System.Windows.Data;

namespace ShoppingCart.WPF.Converters;

/// <summary>
/// Converts a product availability flag into a display status.
/// </summary>
public sealed class BooleanToAvailabilityConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value is true
            ? "Available"
            : "Unavailable";
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}