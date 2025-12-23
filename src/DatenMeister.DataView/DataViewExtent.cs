using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Core.Runtime.Proxies.ReadOnly;

namespace DatenMeister.DataView;

/// <summary>
/// Implements an extent that is based on a data view
/// </summary>
public class DataViewExtent : IUriExtent, IUriResolver, IHasExtentConfiguration
{
    /// <summary>
    /// Stores the element defining the data view
    /// </summary>
    private readonly IElement _dataViewElement;

    /// <summary>
    /// Stores the logic for the data view
    /// </summary>
    private readonly DataViewLogic _dataViewLogic;

    /// <summary>
    /// Stores the extent configuration
    /// </summary>
    private readonly ExtentConfiguration _extentConfiguration;

    /// <summary>
    /// Stores the URL navigator used to resolve elements by URI
    /// </summary>
    private ExtentUrlNavigator _urlNavigator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataViewExtent"/> class
    /// </summary>
    /// <param name="dataViewElement">The element defining the data view</param>
    /// <param name="dataViewLogic">The logic for the data view</param>
    /// <param name="scopeStorage">The scope storage</param>
    public DataViewExtent(IElement dataViewElement, DataViewLogic dataViewLogic, IScopeStorage scopeStorage)
    {
        _dataViewElement = dataViewElement ?? throw new ArgumentNullException(nameof(dataViewElement));
        _dataViewLogic = dataViewLogic ?? throw new ArgumentNullException(nameof(dataViewLogic));
        _urlNavigator = new ExtentUrlNavigator(this, scopeStorage);
        _extentConfiguration = new ExtentConfiguration(this);
    }

    /// <summary>
    /// Gets the extent configuration
    /// </summary>
    public ExtentConfiguration ExtentConfiguration
        => _extentConfiguration
           ?? throw new InvalidOperationException(
               "ExtentConfiguration is not existing");

    /// <inheritdoc />
    public bool equals(object? other) =>
        _dataViewElement.equals(other);

    /// <inheritdoc />
    public object? get(string property) =>
        _dataViewElement.get(property);

    /// <inheritdoc />
    public void set(string property, object? value)
    {
        _dataViewElement.set(property, value);
    }
    
    /// <inheritdoc />
    public T getOrDefault<T>(string property)
    {
        return _dataViewElement.getOrDefault<T>(property);
    }

    /// <inheritdoc />
    public bool isSet(string property) =>
        _dataViewElement.isSet(property);

    /// <inheritdoc />
    public void unset(string property)
    {
        _dataViewElement.unset(property);
    }

    /// <inheritdoc />
    public bool useContainment() =>
        false;

    /// <inheritdoc />
    public IReflectiveSequence elements()
    {
        var itemResult = new PureReflectiveSequence();
        var result = new ReadOnlyReflectiveSequence(itemResult);
        var viewNode = _dataViewElement.getOrDefault<IElement>(_DataViews._DataView.viewNode);
        if (viewNode == null)
        {
            return result;
        }

        return new TemporaryReflectiveSequence(_dataViewLogic.GetElementsForViewNode(viewNode));
    }

    /// <inheritdoc />
    public string contextURI() =>
        _dataViewElement.getOrDefault<string>(_DataViews._DataView.uri);

    /// <inheritdoc />
    public string? uri(IElement element) =>
        element.GetUri();

    /// <inheritdoc />
    public IElement? element(string uri)
    {
        return _urlNavigator.element(uri) as IElement;
    }

    /// <inheritdoc />
    public object? Resolve(string uri, ResolveType resolveType, bool traceFailing = true, string? workspace = null)
    {
        return _urlNavigator.element(uri);
    }

    /// <inheritdoc />
    public IElement? ResolveById(string id)
    {
        return _urlNavigator.element("#" + id) as IElement;
    }
}