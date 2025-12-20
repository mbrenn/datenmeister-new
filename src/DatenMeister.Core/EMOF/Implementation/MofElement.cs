using System;
using System.Threading;
using DatenMeister.Core.EMOF.Implementation.DefaultValue;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;

namespace DatenMeister.Core.EMOF.Implementation;

public class IdIsAlreadySetException(string message) : InvalidOperationException(message);

/// <summary>
/// Defines a Mof Element according to MOF specification
/// </summary>
public class MofElement : MofObject, IElement, IElementSetMetaClass, IHasId, ICanSetId, IKnowsUri
{
    /// <summary>
    /// Stores the cached metaclass to speed-up lookup
    /// </summary>
    private IElement? _cachedMetaClass;

    /// <summary>
    /// Initialized a new instance of the MofElement class which is an abstraction of the provided database.
    /// </summary>
    /// <param name="providedObject">Provided object by database</param>
    /// <param name="extent">Extent to which the object is allocated to</param>
    /// <param name="referenceElement"></param>
    public MofElement(
        IProviderObject providedObject,
        MofExtent? extent,
        IElement? referenceElement = null)
        : base(providedObject, extent, referenceElement)
    {
    }

    /// <summary>
    /// Tracks the number of times the metaclass is accessed.
    /// This static variable is incremented within the implementation of the `getMetaClass` method
    /// to provide a count of how often the metaclass has been retrieved.
    /// </summary>
    private static long _getMetaClassCount = 0;

    /// <summary>
    /// Gets the number of times the metaclass has been retrieved.
    /// </summary>
    public static long GetMetaClassCount => _getMetaClassCount;

    /// <summary>
    /// Tracks the number of times the metaclass is accessed via the ClassModel.
    /// </summary>
    private static long _getMetaClassViaClassModelCount = 0;

    /// <summary>
    /// Gets the number of times the metaclass has been retrieved via ClassModel.
    /// </summary>
    public static long GetMetaClassViaClassModelCount => _getMetaClassViaClassModelCount;

    /// <inheritdoc />
    public IElement? metaclass => getMetaClass();
        
    /// <inheritdoc />
    IElement? IElement.getMetaClass()
    {
        return getMetaClass();
    }

    /// <inheritdoc />
    public IElement? container()
    {
        var containerElement = ProviderObject.GetContainer();
        return containerElement != null
            ? new MofElement(containerElement, Extent ?? ReferencedExtent)
                { Extent = Extent }
            : null;
    }

    /// <summary>
    /// Sets the container for the element
    /// </summary>
    public IObject? Container
    {
        set => ProviderObject.SetContainer(((MofObject?)value)?.ProviderObject);
    }

    /// <summary>
    /// Sets the meta class for the given element
    /// </summary>
    /// <param name="metaClass">Metaclass to be set</param>
    public void SetMetaClass(IElement? metaClass)
    {
        _cachedMetaClass = metaClass is MofElement ? metaClass : null;
        if (metaClass is MofElement mofElement)
        {
            if (mofElement.Extent == null)
            {
                throw new InvalidOperationException("The given metaclass is not connected to an element");
            }

            ProviderObject.MetaclassUri = ((MofUriExtent)mofElement.Extent).uri(mofElement);
        }
        else
        {
            ProviderObject.MetaclassUri = metaClass?.GetUri() ?? string.Empty;
        }

        UpdateContent();
    }

    /// <inheritdoc cref="ICanSetId.Id" />
    public string? Id
    {
        get => ProviderObject.Id;
        set
        {
            if (Id == value)
            {
                // Id is not modified, so we can skip the action
                return;
            }

            if (Extent is MofUriExtent mofUriExtent && !string.IsNullOrEmpty(value))
            {
                var foundValue = mofUriExtent.element($"#{value}");
                if (foundValue != null && !foundValue.Equals(this))
                {
                    throw new IdIsAlreadySetException("The ID is already set within the extent.");
                }
            }

            ProviderObject.Id = value;
        }
    }

    /// <summary>
    /// Sets the referenced extent being used to resolve uris
    /// </summary>
    /// <param name="extent">Extent to be set as start for references</param>
    /// <returns>The element itself for chaining</returns>
    public MofElement SetReferencedExtent(MofExtent extent)
    {
        ReferencedExtent = extent;
        return this;
    }

    /// <summary>
    /// Sets the referenced extent being used to resolve uris
    /// </summary>
    /// <param name="extent">Extent to be set as start for references</param>
    /// <returns>The element itself for chaining</returns>
    public MofElement SetReferencedExtent(IUriExtent extent)
        => SetReferencedExtent((MofExtent)extent);

    public override object? get(string property, bool noReferences, ObjectType objectType)
    {
        var attributeModel = GetClassModel()?.FindAttribute(property);
        // Checks, if we have a dynamic property
        var (isValid, resultValue) = GetDynamicProperty(property);
        if (isValid)
        {
            return ConvertToMofObject(this, property, resultValue, attributeModel, noReferences);
        }
            
        // Checks, if the property is set
        if (ProviderObject.IsPropertySet(property))
        {
            var result = ProviderObject.GetProperty(property, objectType);
            return ConvertToMofObject(this, property, result, attributeModel, noReferences);
        }
        else
        {
            // If not set, get the default value
            var result = GetClassModel()?.FindAttribute(property)?.DefaultValue;
            return ConvertToMofObject(this, property, result, attributeModel, noReferences);
        }
    }

    public IElement? getMetaClass(bool traceFailing = true)
    {
        Interlocked.Increment(ref _getMetaClassCount);
        if (_cachedMetaClass != null && _cachedMetaClass is not MofObjectShadow)
        {
            return _cachedMetaClass;
        }

        var classModel = GetClassModel();
        if (classModel != null)
        {
            if ((ReferencedExtent as IUriResolver)?.Resolve(
                    classModel.Uri,
                    ResolveType.IncludeWorkspace,
                    traceFailing,
                    classModel.WorkspaceId) is IElement localResult)
            {
                Interlocked.Increment(ref _getMetaClassViaClassModelCount);
                return localResult;
            }
        }
        
        var uri = ProviderObject.MetaclassUri;
        
        
        if (uri == null || string.IsNullOrEmpty(uri))
        {
            // No metaclass Uri is given.
            return null;
        }

        var result =
            (ReferencedExtent as IUriResolver)?.Resolve(
                uri,
                ResolveType.IncludeMetaWorkspaces 
                | ResolveType.IncludeTypeWorkspace 
                | ResolveType.IncludeMetaOfMetaWorkspaces,
                traceFailing) as IElement
            ?? new MofObjectShadow(uri);

        _cachedMetaClass = result;
        return result;
    }

    /// <summary>
    /// Sets the metaclass by directly setting the uri
    /// </summary>
    /// <param name="metaClassUri">Uri to be set</param>
    public void SetMetaClass(string metaClassUri)
    {
        _cachedMetaClass = null;
        ProviderObject.MetaclassUri = metaClassUri;

        UpdateContent();
    }

    /// <summary>
    /// Sets the extent by which the element was created
    /// </summary>
    /// <param name="extent">Extent being used to define the reportCreator</param>
    /// <returns>this element. </returns>
    public new IElement CreatedBy(MofExtent extent)
    {
        base.CreatedBy(extent);
        return this;
    }

    /// <summary>
    /// Gets the uri of the element. In case the element is part of an extent, the uri is resolved via extent.
    /// In case the element is not part of an extent, the uri is resolved via the element id directly.
    /// </summary>
    public string Uri
    {
        get
        {
            var extent = this.GetUriExtentOf();
            return extent == null
                ? "#" + Id 
                : extent.uri(this) ?? string.Empty;
        }
    }
}