using System.Xml.Linq;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Repositories;

/// <summary>
/// Provides persistent customer storage using an XML file.
/// </summary>
public sealed class XmlCustomerRepository : ICustomerRepository
{
    private const int InitialCustomerId = 0;

    private readonly string _filePath;

    /// <summary>
    /// Initializes the repository using the default application data location.
    /// </summary>
    public XmlCustomerRepository()
        : this(
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "ShoppingCart",
                "customers.xml"))
    {
    }

    /// <summary>
    /// Initializes the repository using the specified XML file path.
    /// This constructor is useful for unit testing.
    /// </summary>
    /// <param name="filePath">
    /// The path of the XML customer storage file.
    /// </param>
    public XmlCustomerRepository(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "Customer storage file path is required.",
                nameof(filePath));
        }

        _filePath = filePath;
    }

    /// <inheritdoc />
    public Customer? FindByMobileNumber(string mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
        {
            return null;
        }

        var document = LoadDocument();

        var customerElement = document
            .Root?
            .Elements("Customer")
            .FirstOrDefault(
                element =>
                    string.Equals(
                        (string?)element.Element("MobileNumber"),
                        mobileNumber.Trim(),
                        StringComparison.Ordinal));

        return customerElement is null
            ? null
            : MapCustomer(customerElement);
    }

    /// <inheritdoc />
    public int GetNextCustomerId()
    {
        var document = LoadDocument();

        var root = document.Root
                   ?? throw new InvalidOperationException(
                       "The customer XML document does not contain a root element.");

        var lastCustomerId =
            (int?)root.Attribute("LastCustomerId")
            ?? InitialCustomerId;

        var nextCustomerId = lastCustomerId + 1;

        root.SetAttributeValue(
            "LastCustomerId",
            nextCustomerId);

        SaveDocument(document);

        return nextCustomerId;
    }

    /// <inheritdoc />
    public Customer Add(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        var document = LoadDocument();

        var root = document.Root
                   ?? throw new InvalidOperationException(
                       "The customer XML document does not contain a root element.");

        var existingCustomer = root
            .Elements("Customer")
            .FirstOrDefault(
                element =>
                    string.Equals(
                        (string?)element.Element("MobileNumber"),
                        customer.MobileNumber,
                        StringComparison.Ordinal));

        if (existingCustomer is not null)
        {
            throw new InvalidOperationException(
                "A customer with this mobile number already exists.");
        }

        var customerElement = new XElement(
            "Customer",
            new XElement("CustomerId", customer.Id),
            new XElement("Name", customer.Name),
            new XElement("MobileNumber", customer.MobileNumber));

        root.Add(customerElement);

        var currentLastId =
            (int?)root.Attribute("LastCustomerId")
            ?? InitialCustomerId;

        if (customer.Id > currentLastId)
        {
            root.SetAttributeValue(
                "LastCustomerId",
                customer.Id);
        }

        SaveDocument(document);

        return customer;
    }

    /// <summary>
    /// Loads the customer XML document.
    /// Creates the file and directory when necessary.
    /// </summary>
    private XDocument LoadDocument()
    {
        if (!File.Exists(_filePath))
        {
            var document = CreateDocument();

            SaveDocument(document);

            return document;
        }

        return XDocument.Load(_filePath);
    }

    /// <summary>
    /// Creates an empty customer XML document.
    /// </summary>
    private static XDocument CreateDocument()
    {
        return new XDocument(
            new XDeclaration(
                "1.0",
                "utf-8",
                null),

            new XElement(
                "Customers",

                new XAttribute(
                    "LastCustomerId",
                    2),

                new XElement(
                    "Customer",
                    new XElement("CustomerId", 1),
                    new XElement("Name", "Prakash"),
                    new XElement("MobileNumber", "9876543210")),

                new XElement(
                    "Customer",
                    new XElement("CustomerId", 2),
                    new XElement("Name", "Ravi"),
                    new XElement("MobileNumber", "9123456789"))
            ));
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

    /// <summary>
    /// Converts an XML customer element into a domain customer.
    /// </summary>
    private static Customer MapCustomer(
        XElement element)
    {
        return new Customer
        {
            Id =
                (int?)element.Element("CustomerId")
                ?? throw new InvalidOperationException(
                    "Customer ID is missing from XML."),

            Name =
                (string?)element.Element("Name")
                ?? string.Empty,

            MobileNumber =
                (string?)element.Element("MobileNumber")
                ?? string.Empty
        };
    }
}