using System.Xml;
using System.Xml.Linq;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Repositories;

/// <summary>
/// Persists customer orders to an XML file.
/// </summary>
public sealed class XmlOrderRepository : IOrderRepository
{
    private const int InitialOrderId = 1000;

    private readonly string _filePath;

    /// <summary>
    /// Creates the repository using the application's local data folder.
    /// </summary>
    public XmlOrderRepository()
        : this(
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ShoppingCart",
                "orders.xml"))
    {
    }

    /// <summary>
    /// Creates the repository using the specified XML file path.
    /// This constructor also makes the repository easy to test.
    /// </summary>
    public XmlOrderRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        _filePath = filePath;
    }

    /// <inheritdoc />
    public int GetNextOrderId()
    {
        var document = LoadDocument();

        var root = document.Root
                    ?? throw new InvalidOperationException(
                        "The orders XML document does not contain a root element.");

        var lastOrderId =
            (int?)root.Attribute("LastOrderId")
            ?? InitialOrderId;

        var nextOrderId = lastOrderId + 1;

        root.SetAttributeValue("LastOrderId", nextOrderId);

        SaveDocument(document);

        return nextOrderId;
    }

    /// <inheritdoc />
    public void Save(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        var document = LoadDocument();

        var root = document.Root
                    ?? throw new InvalidOperationException(
                        "The orders XML document does not contain a root element.");

        var orderElement =
            new XElement(
                "Order",
                new XElement("OrderId", order.OrderId),
                new XElement("CustomerId", order.CustomerId),
                new XElement(
                    "CreatedAt",
                    XmlConvert.ToString(
                        order.CreatedAt,
                        XmlDateTimeSerializationMode.Local)),
                new XElement(
                    "Products",
                    order.Items.Select(item =>
                        new XElement(
                            "Product",
                            new XElement("ProductId", item.Fruit.Id),
                            new XElement("Quantity", item.Quantity)))));

        root.Add(orderElement);

        SaveDocument(document);
    }

    private XDocument LoadDocument()
    {
        if (!File.Exists(_filePath))
        {
            return CreateDocument();
        }

        return XDocument.Load(_filePath);
    }

    private XDocument CreateDocument()
    {
        var directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return new XDocument(
            new XElement(
                "Orders",
                new XAttribute("LastOrderId", InitialOrderId)));
    }

    private void SaveDocument(XDocument document)
    {
        var directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        document.Save(_filePath);
    }
}