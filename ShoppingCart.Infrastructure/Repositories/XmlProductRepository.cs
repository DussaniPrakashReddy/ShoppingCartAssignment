using System.Xml.Linq;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Repositories;

/// <summary>
/// Provides persistent product catalog storage using XML.
/// </summary>
public sealed class XmlProductRepository : IProductRepository
{
    private const int InitialProductId = 100;

    private readonly string _filePath;

    /// <summary>
    /// Initializes the repository using the default application data location.
    /// </summary>
    public XmlProductRepository()
        : this(
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ShoppingCart",
                "products.xml"))
    {
    }

    /// <summary>
    /// Initializes the repository using the specified XML file path.
    /// This constructor also makes the repository easy to unit test.
    /// </summary>
    /// <param name="filePath">
    /// The XML file used to store products.
    /// </param>
    public XmlProductRepository(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "Product storage file path is required.",
                nameof(filePath));
        }

        _filePath = filePath;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<FruitProduct> GetAll()
    {
        var document = LoadDocument();

        return document
            .Root?
            .Elements("Product")
            .Select(MapProduct)
            .ToList()
            ?? [];
    }

    /// <inheritdoc />
    public int GetNextProductId()
    {
        var document = LoadDocument();

        var root = document.Root
            ?? throw new InvalidOperationException(
                "The product XML document does not contain a root element.");

        var lastProductId =
            (int?)root.Attribute("LastProductId")
            ?? InitialProductId;

        var nextProductId = lastProductId + 1;

        root.SetAttributeValue(
            "LastProductId",
            nextProductId);

        SaveDocument(document);

        return nextProductId;
    }

    /// <inheritdoc />
    public FruitProduct Add(FruitProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var document = LoadDocument();

        var root = document.Root
            ?? throw new InvalidOperationException(
                "The product XML document does not contain a root element.");

        var duplicate = root
            .Elements("Product")
            .FirstOrDefault(
                element =>
                    (int?)element.Element("ProductId") == product.Id);

        if (duplicate is not null)
        {
            throw new InvalidOperationException(
                $"A product with ID {product.Id} already exists.");
        }

        root.Add(MapProductToXml(product));

        UpdateLastProductId(root, product.Id);

        SaveDocument(document);

        return product;
    }

    /// <inheritdoc />
    public void Update(FruitProduct product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var document = LoadDocument();

        var root = document.Root
            ?? throw new InvalidOperationException(
                "The product XML document does not contain a root element.");

        var existingProduct = root
            .Elements("Product")
            .FirstOrDefault(
                element =>
                    (int?)element.Element("ProductId") == product.Id);

        if (existingProduct is null)
        {
            throw new InvalidOperationException(
                $"Product with ID {product.Id} was not found.");
        }

        existingProduct.ReplaceWith(
            MapProductToXml(product));

        SaveDocument(document);
    }

    /// <inheritdoc />
    public void Delete(int productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(productId),
                "Product ID must be greater than zero.");
        }

        var document = LoadDocument();

        var root = document.Root
            ?? throw new InvalidOperationException(
                "The product XML document does not contain a root element.");

        var product = root
            .Elements("Product")
            .FirstOrDefault(
                element =>
                    (int?)element.Element("ProductId") == productId);

        if (product is null)
        {
            return;
        }

        product.Remove();

        SaveDocument(document);
    }

    /// <inheritdoc />
    public FruitProduct? FindById(int productId)
    {
        if (productId <= 0)
        {
            return null;
        }

        var document = LoadDocument();

        var element = document
            .Root?
            .Elements("Product")
            .FirstOrDefault(
                product =>
                    (int?)product.Element("ProductId") == productId);

        return element is null
            ? null
            : MapProduct(element);
    }

    /// <summary>
    /// Loads the XML document.
    /// If the file does not exist, a default catalog is created.
    /// </summary>
    private XDocument LoadDocument()
    {
        if (!File.Exists(_filePath))
        {
            var document = CreateInitialDocument();

            SaveDocument(document);

            return document;
        }

        return XDocument.Load(_filePath);
    }

    /// <summary>
    /// Creates the initial product catalog.
    /// </summary>
    private static XDocument CreateInitialDocument()
    {
        return new XDocument(
            new XDeclaration(
                "1.0",
                "utf-8",
                null),

            new XElement(
                "Products",

                new XAttribute(
                    "LastProductId",
                    104),

                CreateProductElement(
                    101,
                    "Apple",
                    "Fruits",
                    200m,
                    "🍎",
                    true),

                CreateProductElement(
                    102,
                    "Mango",
                    "Fruits",
                    150m,
                    "🥭",
                    true),

                CreateProductElement(
                    103,
                    "Grapes",
                    "Fruits",
                    120m,
                    "🍇",
                    true),

                CreateProductElement(
                    104,
                    "Banana",
                    "Fruits",
                    80m,
                    "🍌",
                    true)
            ));
    }

    /// <summary>
    /// Creates an XML element representing a product.
    /// </summary>
    private static XElement CreateProductElement(
        int id,
        string name,
        string category,
        decimal price,
        string emoji,
        bool isAvailable)
    {
        return new XElement(
            "Product",
            new XElement("ProductId", id),
            new XElement("Name", name),
            new XElement("Category", category),
            new XElement("Price", price),
            new XElement("Emoji", emoji),
            new XElement("IsAvailable", isAvailable));
    }

    /// <summary>
    /// Converts an XML element into a product entity.
    /// </summary>
    private static FruitProduct MapProduct(XElement element)
    {
        return new FruitProduct
        {
            Id =
                (int?)element.Element("ProductId")
                ?? throw new InvalidOperationException(
                    "Product ID is missing from XML."),

            Name =
                (string?)element.Element("Name")
                ?? string.Empty,

            Category =
                (string?)element.Element("Category")
                ?? string.Empty,

            Price =
                (decimal?)element.Element("Price")
                ?? 0m,

            Emoji =
                (string?)element.Element("Emoji")
                ?? string.Empty,

            IsAvailable =
                (bool?)element.Element("IsAvailable")
                ?? true
        };
    }

    /// <summary>
    /// Converts a product entity into an XML element.
    /// </summary>
    private static XElement MapProductToXml(
        FruitProduct product)
    {
        return CreateProductElement(
            product.Id,
            product.Name,
            product.Category,
            product.Price,
            product.Emoji,
            product.IsAvailable);
    }

    /// <summary>
    /// Updates the last generated product identifier.
    /// </summary>
    private static void UpdateLastProductId(
        XElement root,
        int productId)
    {
        var currentLastId =
            (int?)root.Attribute("LastProductId")
            ?? InitialProductId;

        if (productId > currentLastId)
        {
            root.SetAttributeValue(
                "LastProductId",
                productId);
        }
    }

    /// <summary>
    /// Saves the XML document to disk.
    /// </summary>
    private void SaveDocument(XDocument document)
    {
        var directory =
            Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        document.Save(_filePath);
    }
}