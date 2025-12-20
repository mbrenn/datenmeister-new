using System.Collections;
using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly.Model;

namespace DatenMeister.Core.EMOF.Implementation;

/// <summary>
/// Implements the extent interface according the MOF specification
/// </summary>
public class MofExtent : 
    IExtent, IHasWorkspace, IObjectAllProperties, IHasExtent,
    IHasMofExtentMetaObject, IHasExtentConfiguration
{
    /// <summary>
    /// Stores the logging instance
    /// </summary>
    public static readonly ILogger logger = new ClassLogger(typeof(MofExtent));
    
    private static readonly MofExtent XmlMetaExtent =
        new MofUriExtent(new XmiProvider(), null)
        {
            LocalSlimUmlEvaluation = true
        };

    /// <summary>
    /// Stores a list of other extents that shall also be considered as meta extents
    /// </summary>
    private readonly List<IExtent> _metaExtents = [];

    /// <summary>
    /// STores the object for synchronization issues
    /// </summary>
    private readonly Lock _syncObject = new();

    /// <summary>
    /// Stores the information whether the evaluation about the item count is currently
    /// running in the background. 
    /// </summary>
    private bool _isItemCountRunning;

    private int _itemCountCached;

    /// <summary>
    /// Initializes a new instance of the Extent
    /// </summary>
    /// <param name="provider">Provider being used for the extent</param>
    /// <param name="scopeStorage">Scope storage to be used to find Change Event Manager</param>
    public MofExtent(IProvider provider, IScopeStorage? scopeStorage = null)
    {
        ChangeEventManager = scopeStorage?.TryGet<ChangeEventManager>();
        ScopeStorage = scopeStorage;

        var rootProvider = new XmiProvider();
        Provider = provider;
        TypeLookup = new DotNetTypeLookup();

        // Defines the Meta Element. 
        // The pseudo extent is used to allow the factory to create the correct instance of Xml
        // instead of the specific type. 
        if (XmlMetaExtent != null)
        {
            MetaXmiElement = new MofElement(
                rootProvider.CreateProviderObject(new XElement("meta")),
                XmlMetaExtent);
            XmlMetaExtent.AddMetaExtent(this);
        }
        else
        {
            MetaXmiElement = new MofElement(
                rootProvider.CreateProviderObject(new XElement("meta")),
                this);
        }

        MetaXmiElement.SetMetaClass(_Management.TheOne.__ExtentProperties);

        ExtentConfiguration = new ExtentConfiguration(this);
    }

    /// <summary>
    /// Stores the configuration for the extent
    /// </summary>
    public ExtentConfiguration ExtentConfiguration { get; }

    /// <summary>
    /// This type lookup can be used to convert the instances of the .Net types to real MOF meta classes.
    /// It is only used, if the data is directly set as a .Net object
    /// </summary>
    public IDotNetTypeLookup TypeLookup { get; }

    /// <summary>
    /// Gets or sets the provider for the given extent
    /// </summary>
    public IProvider Provider { get; }

    /// <summary>
    /// Defines the scope storage in which the extent is working
    /// </summary>
    public IScopeStorage? ScopeStorage { get; private set; }

    /// <summary>
    /// Gets a value whether the extent will support the full Uml capabilities regarding auto enumeration
    /// and default value of properties. For certain extents, especially 'just getting loaded' extents,
    /// the slim evaluation might bring shorter execution times 
    /// </summary>
    public bool LocalSlimUmlEvaluation { get; set; }

    /// <summary>
    /// Gets or sets the flag whether the slim evaluation is activated globally
    /// </summary>
    public static bool GlobalSlimUmlEvaluation { get; set; }

    /// <summary>
    /// Gets or sets the effective slim evaluation
    /// </summary>
    public bool SlimUmlEvaluation => LocalSlimUmlEvaluation || GlobalSlimUmlEvaluation;
    /// <summary>
    /// Gets the workspace to which the extent is allocated
    /// </summary>
    public IWorkspace? Workspace { get; set; }

    /// <summary>
    /// Gets or sets the change event manager for the objects within
    /// </summary>
    internal ChangeEventManager? ChangeEventManager { get; set; }

    /// <summary>
    /// Gets or sets the flag indicating whether the extent is modified
    /// </summary>
    public bool IsModified { get; private set; }

    public int ItemCount
    {
        get
        {
            lock (_syncObject)
            {
                if (_itemCountCached == -1 && !_isItemCountRunning)
                {
                    _isItemCountRunning = true;
                    Task.Run(() =>
                    {
                        //_itemCountCached = elements().GetAllCompositesIncludingThemselves().size();
                        ChangeEventManager?.SendChangeEvent(this);
                    });
                }

                return _itemCountCached;
            }
        }
    }

    /// <summary>
    /// Gets the meta element for xmi data
    /// </summary>
    public MofElement MetaXmiElement { get; set; }

    /// <summary>
    /// Gets or sets the xml Node of the meta element.
    /// Only to be used by ExtentConfigurationLoader and other extent loaders.
    /// This method shall only be called, if the underlying provider does not support storage of metadata
    /// </summary>
    public XElement LocalMetaElementXmlNode
    {
        get => ((XmiProviderObject) MetaXmiElement.ProviderObject).XmlNode;
        set => ((XmiProviderObject) MetaXmiElement.ProviderObject).XmlNode = value;
    }

    /// <summary>
    /// Gets the meta extents
    /// </summary>
    public IEnumerable<IExtent> MetaExtents
    {
        get
        {
            lock (_metaExtents)
            {
                return _metaExtents.ToList();
            }
        }
    }

    /// <inheritdoc />
    public bool equals(object? other)
    {
        if (other is MofExtent otherAsExtent)
        {
            return Equals(otherAsExtent);
        }

        return false;
    }

    /// <inheritdoc />
    public object? get(string property)
    {
        if (property == _UML._Packages._Package.packagedElement)
        {
            return elements();
        }

        if (Provider.GetCapabilities().StoreMetaDataInExtent)
        {
            var nullObject = Provider.Get(null) ??
                             throw new InvalidOperationException(
                                 "Provider does not support setting of extent properties");
            return MofObject.ConvertToMofObject(
                new MofObject(nullObject, this),
                property,
                nullObject.GetProperty(property),
                null /* No Attribute Model available for extents*/);
        }

        return MetaXmiElement.get(property);
    }

    public T getOrDefault<T>(string property)
    {
        return ObjectHelper.getOrDefault<T>(this, property);
    }

    /// <inheritdoc />
    public void set(string property, object? value)
    {
        if (Provider.GetCapabilities().StoreMetaDataInExtent)
        {
            var nullObject = Provider.Get(null) ??
                             throw new InvalidOperationException(
                                 "Provider does not support setting of extent properties");

            if (DotNetHelper.IsOfEnumeration(value))
            {
                if (value == null) throw new InvalidOperationException("value == null");

                nullObject.EmptyListForProperty(property);
                foreach (var child in (IEnumerable<object>) value)
                {
                    var valueForSetting = ConvertForSetting(this, child, null);
                    if (valueForSetting != null)
                    {
                        nullObject.AddToProperty(property, valueForSetting);
                    }
                }
            }
            else
            {
                var valueForSetting = ConvertForSetting(this, value, null);
                nullObject.SetProperty(property, valueForSetting);
            }
        }
        else
        {
            MetaXmiElement.set(property, value);
        }

        SignalUpdateOfContent();
    }

    /// <inheritdoc />
    public bool isSet(string property)
    {
        if (Provider.GetCapabilities().StoreMetaDataInExtent)
        {
            var nullObject = Provider.Get(null) ??
                             throw new InvalidOperationException(
                                 "Provider does not support setting of extent properties");
            return nullObject.IsPropertySet(property);
        }

        return MetaXmiElement.isSet(property);
    }

    /// <inheritdoc />
    public void unset(string property)
    {
        if (Provider.GetCapabilities().StoreMetaDataInExtent)
        {
            var nullObject = Provider.Get(null) ??
                             throw new InvalidOperationException(
                                 "Provider does not support setting of extent properties");

            nullObject.DeleteProperty(property);
        }
        else
        {
            MetaXmiElement.unset(property);
        }

        SignalUpdateOfContent();
    }

    /// <inheritdoc />
    public bool useContainment() => false;

    /// <inheritdoc />
    public IReflectiveSequence elements() => new ExtentReflectiveSequence(this);

    public IExtent Extent => this;

    /// <summary>
    /// Gets the meta object representing the meta object. Setting, querying a list or getting
    /// is supported by this object
    /// </summary>
    /// <returns>The returned value representing the meta object</returns>
    public MofObject GetMetaObject()
    {
        if (Provider.GetCapabilities().StoreMetaDataInExtent)
        {
            IProviderObject nullObject;
                
            lock (_syncObject)
            {
                nullObject = Provider.Get(null) ??
                             throw new InvalidOperationException(
                                 "Provider does not support setting of extent properties");
            }

            var result = new MofElement(nullObject, this);
            result.SetMetaClass(_Management.TheOne.__ExtentProperties);
            return result;
        }

        return MetaXmiElement;
    }

    /// <summary>
    /// Gets the workspace via the interface
    /// </summary>
    IWorkspace? IHasWorkspace.Workspace => Workspace;

    /// <summary>
    /// Gets all the properties that are set in the meta information of the extent
    /// </summary>
    /// <returns>Enumeration of strings</returns>
    public IEnumerable<string> getPropertiesBeingSet()
    {
        if (Provider.GetCapabilities().StoreMetaDataInExtent)
        {
            var nullObject = Provider.Get(null) ??
                             throw new InvalidOperationException(
                                 "Provider does not support setting of extent properties");
            return nullObject.GetProperties();
        }

        return MetaXmiElement.getPropertiesBeingSet();
    }

    /// <summary>
    /// Adds an extent as a meta extent, so it will also be used to retrieve the element
    /// </summary>
    /// <param name="extent">Extent to be added</param>
    public void AddMetaExtent(IExtent extent)
    {
        lock (_metaExtents)
        {
            if (extent is IUriExtent uriExtent)
            {
                _metaExtents.RemoveAll(x => (x as IUriExtent)?.contextURI() == uriExtent.contextURI());
            }

            if (_metaExtents.Any(x => x.Equals(extent))) return;

            _metaExtents.Add(extent);
        }

        if (XmlMetaExtent != this)
        {
            XmlMetaExtent.AddMetaExtent(extent);
        }
    }

    /// <summary>
    /// Adds a number of extents as meta extent to the given extent
    /// </summary>
    /// <param name="extents"></param>
    public void AddMetaExtents(IEnumerable<IExtent> extents)
    {
        foreach (var extent in extents.OfType<IUriExtent>())
        {
            AddMetaExtent(extent);
        }
    }

    /// <summary>
    /// Resolves the DotNetType by navigating through the current and the meta instances.
    /// </summary>
    /// <param name="metaclassUri">Uri class to be retrieved</param>
    /// <param name="resolveType">The resolving strategy</param>
    /// <returns>Resolved .Net Type as IElement</returns>
    public Type? ResolveDotNetType(string metaclassUri, ResolveType resolveType)
    {
        if (resolveType.HasFlagFast(ResolveType.IncludeExtent))
        {
            var result = TypeLookup.ToType(metaclassUri);
            if (result != null)
            {
                return result;
            }
        }

        // Now look into the explicit extents
        if (resolveType.HasFlagFast(ResolveType.IncludeMetaWorkspaces))
        {
            foreach (var metaExtent in MetaExtents.Cast<MofUriExtent>())
            {
                var element = metaExtent.TypeLookup.ToType(metaclassUri);
                if (element != null)
                {
                    return element;
                }
            }
        }

        var resolve = ResolveDotNetTypeByMetaWorkspaces(metaclassUri, Workspace);
        return resolve;
    }

    /// <summary>
    /// Resolves the the given uri by looking through each meta workspace of the workspace
    /// </summary>
    /// <param name="metaclassUri">Uri being retrieved</param>
    /// <param name="workspace">Workspace whose meta workspaces were queried</param>
    /// <param name="alreadyVisited">Set of all workspaces already being visited. This avoid unnecessary recursion and unlimited recursion</param>
    /// <returns>Found element or null, if not found</returns>
    private Type? ResolveDotNetTypeByMetaWorkspaces(
        string metaclassUri,
        IWorkspace? workspace,
        HashSet<IWorkspace>? alreadyVisited = null)
    {
        alreadyVisited ??= [];
        if (workspace != null && alreadyVisited.Contains(workspace))
        {
            return null;
        }

        if (workspace != null)
        {
            alreadyVisited.Add(workspace);
        }

        // If still not found, look into the meta workspaces. Nevertheless, no recursion
        var metaWorkspaces = workspace?.MetaWorkspaces;
        if (metaWorkspaces != null)
        {
            foreach (var metaWorkspace in metaWorkspaces)
            {
                foreach (var metaExtent in metaWorkspace.extent.OfType<MofUriExtent>())
                {
                    var element = metaExtent.TypeLookup.ToType(metaclassUri);
                    if (element != null)
                    {
                        return element;
                    }
                }

                var elementByMeta = ResolveDotNetTypeByMetaWorkspaces(metaclassUri, metaWorkspace, alreadyVisited);
                if (elementByMeta != null)
                {
                    return elementByMeta;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the uri of the metaclass by looking through current extent, meta extent and meta workspaces
    /// </summary>
    /// <param name="type">Type to be converted</param>
    /// <returns>Retrieved meta class uri</returns>
    public string? GetMetaClassUri(Type type)
    {
        var result = TypeLookup.ToElement(type);
        if (!string.IsNullOrEmpty(result))
        {
            return result;
        }

        // Now look into the explicit extents
        foreach (var metaExtent in MetaExtents.OfType<MofExtent>())
        {
            var element = metaExtent.TypeLookup.ToElement(type);
            if (!string.IsNullOrEmpty(element))
            {
                return element;
            }
        }

        // If still not found, look into the meta workspaces. Nevertheless, no recursion
        var metaWorkspaces = Workspace?.MetaWorkspaces;
        if (metaWorkspaces != null)
        {
            foreach (var metaWorkspace in metaWorkspaces)
            {
                var metaClassUri = WorkspaceDotNetHelper.GetMetaClassUriOfDotNetType(metaWorkspace, type);
                if (metaClassUri != null) return metaClassUri;
            }
        }

        return null;
    }

    /// <summary>
    /// Converts the given value to a value which can be used for the provoder
    /// </summary>
    /// <param name="value">Value to be converted</param>
    /// <returns>Converted value</returns>
    public static object ConvertForProviderUsage(object value)
    {
        if (value is MofObject asMofObject)
        {
            return asMofObject.ProviderObject;
        }

        return value;
    }

    /// <summary>
    /// Converts the object to be set by the data provider. This is the inverse object to ConvertToMofObject.
    /// An arbitrary object shall be stored into the database
    /// </summary>
    /// <param name="value">Value to be converted</param>
    /// <param name="attributeModel">Attribute model being used to convert the value</param>
    /// <returns>The converted object or an exception if the object cannot be converted</returns>
    public object? ConvertForSetting(object value, AttributeModel? attributeModel)
        => ConvertForSetting(value, this, attributeModel);

    /// <summary>
    /// Converts the given value to an element that can be used be for the provider object
    /// </summary>
    /// <param name="value">Value to be set</param>
    /// <param name="extent">Extent being used to create the factory or being used for .Net TypeLookup</param>
    /// <param name="attributeModel">Attribute model being used to convert the value</param>
    /// <returns>The converted object being ready for Provider</returns>
    private static object? ConvertForSetting(object? value, MofExtent? extent, AttributeModel? attributeModel)
    {
        var isComposite = attributeModel?.IsComposite;
        
        if (value == null)
        {
            return null;
        }

        if (DotNetHelper.IsOfPrimitiveType(value) || DotNetHelper.IsOfEnum(value))
        {
            return value;
        }

        if (DotNetHelper.IsOfMofShadow(value))
        {
            var asMofObjectShadow = (MofObjectShadow) value;

            // It is a reference
            var reference = new UriReference(asMofObjectShadow.Uri);

            return reference;
        }

        if (DotNetHelper.IsUriReference(value))
        {
            return value;
        }

        if (DotNetHelper.IsOfUriExtent(value))
        {
            if (value is not IUriExtent ofUriExtent)
                throw new InvalidOperationException("Should not exist");

            return new UriReference(ofUriExtent.contextURI());
        }

        if (DotNetHelper.IsOfMofObject(value))
        {
            if (extent == null)
                throw new InvalidOperationException("Extent is null but MofObject given");

            var asMofObject = (MofObject) value;

            if (asMofObject.Extent == null || isComposite == true)
            {
                if (isComposite == false)
                {
                    var extentUri = (extent as IUriExtent)?.contextURI() ?? "Unknown";
                    var attributeName = attributeModel?.Name ?? "Unknown";
                    logger.Info($"We are NOT composite, but would like to get added without knowing the extent: {asMofObject} to Extent: {extentUri} (Attribute: {attributeName})");
                }
                
                if (asMofObject.ProviderObject.Provider == extent.Provider)
                {
                    // if the given value is created by the provider, but has not been allocated
                    // to an object until now, it can be used directly.
                    return asMofObject.ProviderObject;
                }
                
                // We are having (1) a composite object or (2) no information about composition strategy
                // In case of (1), we clone the item, since we have to. The id is reused since the item is not
                // added anywhere
                // In case of (2), we clone the item, since we assume that it needs to be copied
                
                // If the object to be added is not connected to an extent, we have to copy 
                // the element to the right provider type, but we keep the id
                var result = (MofElement) ObjectCopier.Copy(
                    new MofFactory(extent),
                    asMofObject, 
                    new CopyOption {CopyId = true});
                return result.ProviderObject;
            }

            // We regard it as a reference since no composition is requested
            var asElement = asMofObject as IElement ??
                            throw new InvalidOperationException("Given element is not of type IElement");
            var uriExtentOfItem = (MofUriExtent)asMofObject.Extent;
            var workspace = uriExtentOfItem.GetWorkspace()?.id ?? string.Empty;

            return new UriReference(((MofUriExtent)asMofObject.Extent).uri(asElement))
            {
                Workspace = workspace
            };
        }

        if (DotNetHelper.IsOfEnumeration(value))
        {
            return ((IEnumerable) value)
                .Cast<object>()
                .Select(innerValue => ConvertForSetting(innerValue, extent, attributeModel)).ToList();
        }

        if (DotNetHelper.IsOfProviderObject(value))
        {
            throw new InvalidOperationException(
                "Setting of IProviderObjects is not supported. Create a MofObject containing that element");
        }

        // Then, we have a simple dotnet type, that we try to convert. Let's hope, that it works
        if (extent is not IUriExtent asUriExtent)
        {
            throw new InvalidOperationException(
                "This element was not created by a factory. So a setting by .Net Object is not possible");
        }

        // In case we have a pure .Net Object perform the conversion directly
        return ConvertForSetting(
            DotNetConverter.ConvertToMofObject(asUriExtent, value), extent, attributeModel);
    }

    /// <summary>
    /// Converts the object to be set by the data provider. This is the inverse object to ConvertToMofObject.
    /// An arbitrary object shall be stored into the database
    /// </summary>
    /// <param name="recipient">The Mofobject for which the element will be created</param>
    /// <param name="childValue">Value to be converted</param>
    /// <param name="attributeModel">The attribute model describing the properties of the attribute</param>
    /// <returns>The converted object or an exception if the object cannot be converted</returns>
    public static object? ConvertForSetting(IObject recipient, object? childValue, AttributeModel? attributeModel)
    {
        ArgumentNullException.ThrowIfNull(recipient);

        switch (recipient)
        {
            case MofObject mofObject:
            {
                var result = ConvertForSetting(childValue, mofObject.ReferencedExtent, attributeModel);

                if (result is IProviderObject && childValue is MofObject childValueAsObject)
                {
                    // Sets the extent of the newly added object which will be associated to the mofObject
                    // This value must be set, so the new information is propagated to the MofObjects
                    childValueAsObject.ReferencedExtent = mofObject.Extent ?? mofObject.ReferencedExtent;
                    childValueAsObject.Extent = mofObject.Extent;
                }

                return result;
            }
            case MofExtent extent:
            {
                var result = ConvertForSetting(childValue, extent, attributeModel);

                if (result is IProviderObject && childValue is MofObject childValueAsObject)
                {
                    // Sets the extent of the newly added object which will be associated to the mofObject
                    // This value must be set, so the new information is propagated to the MofObjects
                    childValueAsObject.ReferencedExtent = extent;
                    childValueAsObject.Extent = extent;
                }

                return result;
            }
            default:
                throw new InvalidOperationException("Type of ${value.GetType()} is not known");
        }
    }

    /// <summary>
    /// Indicates that the content of the extent is updated 
    /// </summary>
    /// <param name="isModified">true, if the modification flag shall be set to true</param>
    public void SignalUpdateOfContent(bool isModified = true)
    {
        _itemCountCached = -1;
        IsModified = isModified;
    }
}