using ShoppingCart.WPF.Services;

namespace ShoppingCart.Tests.Fakes;

public sealed class FakeWindowNavigationService : IWindowNavigationService
{
    public bool OpenProductsWindowCalled { get; private set; }

    public void OpenProductsWindow()
    {
        OpenProductsWindowCalled = true;
    }
}