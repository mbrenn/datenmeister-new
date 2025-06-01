using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Proxies;
using DatenMeister.Core.Runtime.Proxies.ReadOnly;

namespace DatenMeister.DataView;

public class DataViewExtent : IUriExtent, IUriResolver
{
    private readonly IElement _dataViewElement;
    private readonly DataViewLogic _dataViewLogic;

    private ExtentUrlNavigator _urlNavigator; 

    public DataViewExtent(IElement dataViewElement, DataViewLogic dataViewLogic, IScopeStorage scopeStorage)
    {
        _dataViewElement = dataViewElement ?? throw new ArgumentNullException(nameof(dataViewElement));
        _dataViewLogic = dataViewLogic ?? throw new ArgumentNullException(nameof(dataViewLogic));
        _urlNavigator = new ExtentUrlNavigator(this, scopeStorage);
    }

    public bool equals(object? other) =>
        _dataViewElement.equals(other);

    public object? get(string property) =>
        _dataViewElement.get(property);

    public void set(string property, object? value)
    {
        _dataViewElement.set(property, value);
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