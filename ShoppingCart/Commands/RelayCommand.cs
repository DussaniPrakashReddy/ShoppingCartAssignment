using System.Windows.Input;

namespace ShoppingCart.WPF.Commands;

/// <summary>
/// Represents a command that delegates execution and
/// CanExecute evaluation to supplied delegates.
/// </summary>
public sealed class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">
    /// Action executed by the command.
    /// </param>
    /// <param name="canExecute">
    /// Predicate determining whether the command can execute.
    /// </param>
    public RelayCommand(
        Action<object?> execute,
        Predicate<object?>? canExecute = null)
    {
        _execute =
            execute ?? throw new ArgumentNullException(nameof(execute));

        _canExecute = canExecute;
    }

    /// <inheritdoc />
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    /// <summary>
    /// Notifies WPF that the command's CanExecute state may have changed.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(
            this,
            EventArgs.Empty);
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;
}