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

public class DataViewExtent : IUriExtent, IUriResolver, IHasExtentConfiguration
{
    private readonly IElement _dataViewElement;
    private readonly DataViewLogic _dataViewLogic;

    private readonly ExtentConfiguration _extentConfiguration;

    private ExtentUrlNavigator _urlNavigator;

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

    public bool equals(object? other) =>
        _dataViewElement.equals(other);

    public object? get(string property) =>
        _dataViewElement.get(property);

    public void set(string property, object? value)
    {
        _dataViewElement.set(property, value);
    }
    
    public T getOrDefault<T>(string property)
    {
        return _dataViewElement.getOrDefault<T>(property);
    }

    public bool isSet(string property) =>
        _dataViewElement.isSet(property);

    public void unset(string property)
    {
        _dataViewElement.unset(property);
    }

    public bool useContainment() =>
        false;

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

    public string contextURI() =>
        _dataViewElement.getOrDefault<string>(_DataViews._DataView.uri);

    public string? uri(IElement element) =>
        element.GetUri();

    public IElement? element(string uri)
    {
        return _urlNavigator.element(uri) as IElement;
    }


    public object? Resolve(string uri, ResolveType resolveType, bool traceFailing = true, string? workspace = null)
    {
        return _urlNavigator.element(uri);
    }


    public IElement? ResolveById(string id)
    {
        return _urlNavigator.element("#" + id) as IElement;
    }
}