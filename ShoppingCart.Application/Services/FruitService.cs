using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.Services.Interfaces;
using ShoppingCart.Domain.Interfaces;

namespace ShoppingCart.Application.Services;

/// <summary>
/// Implements application operations related to fruits.
/// </summary>
public sealed class FruitService : IFruitService
{
    private readonly IFruitRepository _fruitRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="FruitService"/>.
    /// </summary>
    /// <param name="fruitRepository">
    /// Repository used to retrieve available fruits.
    /// </param>
    public FruitService(IFruitRepository fruitRepository)
    {
        _fruitRepository = fruitRepository;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IFruit> GetAvailableFruits()
    {
        return _fruitRepository.GetAll();
    }

}