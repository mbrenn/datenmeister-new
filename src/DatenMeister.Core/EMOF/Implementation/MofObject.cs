using System.Diagnostics;
using DatenMeister.Core.EMOF.Implementation.Uml;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation;

/// <summary>
/// Implements the abstraction of the Mof Object.
/// </summary>
public class MofObject : IObject, IHasExtent, IObjectAllProperties, IHasMofExtentMetaObject
{
    /// <summary>
    /// Stores the extent
    /// </summary>
    private MofExtent? _extent;

    /// <summary>
    /// Stores the referenced extent
    /// </summary>
    private MofExtent? _referencedExtent;

    /// <summary>
    /// Gets the extent of the mof object
    /// </summary>
    public MofExtent? Extent
    {
        get => _extent;
        set
        {
            if (value == null)
            {
                _extent = null;
            }
            else
            {
                _extent = ReferencedExtent = value;
            }
        }
    }

    /// <summary>
    /// Stores the extent that is used to create the element.
    /// This extent is used for type lookup and other referencing things.
    /// </summary>
    public MofExtent ReferencedExtent
    {
        get => _referencedExtent ?? throw new InvalidOperationException("Referenced Extent is not set");
        set => _referencedExtent = value;
    }

    /// <summary>
    /// Gets the extent of the mof object
    /// </summary>
    IExtent? IHasExtent.Extent => Extent;

    /// <summary>
    /// Gets the provided object
    /// </summary>
    public IProviderObject ProviderObject { get; }

    /// <summary>
    /// Gets or sets the value whether we are having a cached slim evaluation
    /// </summary>
    private bool? _cachedIsSlimEvaluation = null;
    
    /// <summary>
    /// Gets or sets the flag whether slim uml evaluation is activated
    /// </summary>
    public bool IsSlimUmlEvaluation
    {
        get
        {
            if (_cachedIsSlimEvaluation.HasValue)
            {
                return _cachedIsSlimEvaluation.Value;
            }

            if (ReferencedExtent.SlimUmlEvaluation)
            {
                _cachedIsSlimEvaluation = true;
                return true;
            }
            
            _cachedIsSlimEvaluation =
                !(((IObject) this).GetExtentOf() as MofExtent)?.SlimUmlEvaluation == false;

            return _cachedIsSlimEvaluation.Value;
        }
    }
        

    /// <summary>
    /// Initializes a new instance of the MofObject class.
    /// </summary>
    /// <param name="providedObject">The database abstraction of the object</param>
    /// <param name="referencedExtent">The extent being used to access the item</param>
    /// <param name="referenceElement"></param>
    public MofObject(IProviderObject providedObject, MofExtent? referencedExtent, IElement? referenceElement = null)
    {
        ProviderObject = providedObject ?? throw new ArgumentNullException(nameof(providedObject));

        if (providedObject.Provider == null)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("providedObject.Provider");
        }

        ReferencedExtent =
            referencedExtent
            ?? ((referenceElement as MofObject)?.ReferencedExtent
                ?? throw new InvalidOperationException("Referenced extent could not be set"));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => equals(obj);

    /// <summary>
    /// Verifies if the two elements reference to the same instance
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool AreEqual(IObject? first, IObject? second)
    {
        if (first == null && second == null)
        {
            return true;
        }
            
        if (first == null || second == null)
        {
            // If one is at least null, it shall be
            return false;
        }

        var firstAsMofObject = first as MofObject;
        var secondAsMofObject = second as MofObject;
        var firstAsShadow = first as MofObjectShadow;
        var secondAsShadow = second as MofObjectShadow;
        var firstAsElement = first as MofElement;
        var secondAsElement = second as MofElement;
        var firstAsUriExtent = first as MofUriExtent;
        var secondAsUriExtent = second as MofUriExtent;

        if (firstAsMofObject != null && secondAsMofObject != null)
        {
            return firstAsMofObject.ProviderObject.Id == secondAsMofObject.ProviderObject.Id;
        }

        if (firstAsShadow != null && secondAsShadow != null)
        {
            return firstAsShadow.Uri == secondAsShadow.Uri;
        }

        if (firstAsShadow != null && secondAsElement != null)
        {
            return firstAsShadow.Uri == secondAsElement.GetUri();
        }

        if (secondAsShadow != null && firstAsElement != null)
        {
            return secondAsShadow.Uri == firstAsElement.GetUri();
        }

        if (firstAsUriExtent != null && secondAsUriExtent != null)
        {
            // Context uri of both are equal
            return firstAsUriExtent.contextURI() == secondAsUriExtent.contextURI();
        }

        if (firstAsUriExtent != null || secondAsUriExtent != null)
        {
            // One is a uri extent but the other one is not, so it is sure that both are not equal
            return false;
        }

        throw new InvalidOperationException(
            $"Combination of {first.GetType()} and {second.GetType()} is not known to verify equality");
    }

    /// <inheritdoc />
    public bool equals(object? other)
        => AreEqual(this, other as IObject);

    /// <inheritdoc />
    // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
    public override int GetHashCode() => ProviderObject.GetHashCode();

    /// <inheritdoc />
    public object? get(string property)
        => get(property, false, ObjectType.None);

    // ReSharper disable once InconsistentNaming
    public virtual object? get(string property, bool noReferences, ObjectType objectType)
    {
        // Checks, if we have a dynamic property
        var (isValid, resultValue) = GetDynamicProperty(property);
        if (isValid)
        {
            return ConvertToMofObject(this, property, resultValue, noReferences);
        }
            
        // If not, return the item from the database
        var result = ProviderObject.GetProperty(property, objectType);
        return ConvertToMofObject(this, property, result, noReferences);
    }

    protected virtual (bool, object?) GetDynamicProperty(string property)
    {
        return (false, null);
    }

    /// <summary>
    /// Converts the object to a mof object that can be added to the MofObject
    /// </summary>
    /// <param name="container">Container to be added</param>
    /// <param name="property">Property to be set</param>
    /// <param name="value">Value to be converted</param>
    /// <param name="noReferences">True, if references shall be resolved</param>
    /// <returns>The converted object</returns>
    internal static object? ConvertToMofObject(
        MofObject container,
        string property,
        object? value,
        bool noReferences = false)
    {
        if (value == null)
        {
            return null;
        }

        if (DotNetHelper.IsOfPrimitiveType(value) || DotNetHelper.IsOfEnum(value))
        {
            return value;
        }

        switch (value)
        {
            case IProviderObject resultAsProviderObject:
            {
                var result = new MofElement(resultAsProviderObject, container.ReferencedExtent, container as MofElement)
                {
                    Extent = container.Extent
                };

                return result;
            }
            case IEnumerable<object> _:
                return new MofReflectiveSequence(container, property);
            case UriReference valueAsUriReference when noReferences:
                return valueAsUriReference;
            case UriReference valueAsUriReference:
            {
                var extentResolver = container.Extent as IUriResolver ?? container.ReferencedExtent as IUriResolver;
                var resolved = extentResolver?.Resolve(valueAsUriReference.Uri, ResolveType.Default, false, valueAsUriReference.Workspace);
                return resolved ?? new MofObjectShadow(valueAsUriReference.Uri);
            }
            default:
                throw new NotImplementedException($"Type of {value.GetType()} currently not supported.");
        }
    }


    /// <inheritdoc />
    public void set(string property, object? value)
    {
        // Checks if the value is a default value. If yes, it can be removed...
        if (MofHelper.IsDefaultValue(this, property, value))
        {
            ProviderObject.DeleteProperty(property);
            return;
        }

        // Value is not a default value, so it needs to be stored into the database
        if (DotNetHelper.IsOfEnumeration(value))
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
                
            var valueAsEnumeration = (IEnumerable<object>) value;
            ProviderObject.EmptyListForProperty(property);
            foreach (var child in valueAsEnumeration)
            {
                var valueForSetting = MofExtent.ConvertForSetting(this, child);
                if (valueForSetting == null)
                {
                    // Null elements will not be set
                    continue;
                }
                    
                ProviderObject.AddToProperty(property, valueForSetting);

                // Checks, if the element that has been set is not associated to a container.
                // If the element is not associated, set the container.
                if (valueForSetting is IProviderObject valueAsProviderObject &&
                    !valueAsProviderObject.HasContainer())
                {
                    SetContainer(ProviderObject, valueForSetting);
                }
            }
        }
        else
        {
            var valueForSetting = MofExtent.ConvertForSetting(this, value);
            ProviderObject.SetProperty(property, valueForSetting);

            // Checks, if the element that has been set is not associated to a container.
            // If the element is not associated, set the container.
            if (valueForSetting is IProviderObject valueAsProviderObject &&
                !valueAsProviderObject.HasContainer())
            {
                SetContainer(ProviderObject, valueForSetting);
            }

            if (value is MofObject ofMofObject 
                && valueForSetting is UriReference 
                && ofMofObject.ReferencedExtent is IUriExtent asUriExtent)
            {
                _referencedExtent?.AddMetaExtent(asUriExtent);
            }
        }

        UpdateContent();
    }

    /// <summary>
    /// Sets the container of the child object to this instance
    /// </summary>
    /// <param name="parentProviderObject">The parent object containing the child after the allocation</param>
    /// <param name="childAsProviderObject">Child as potential provider object</param>
    internal static void SetContainer(IProviderObject parentProviderObject, object childAsProviderObject)
    {
        if (childAsProviderObject is IProviderObject childProviderObject)
        {
            if (/*!childProviderObject.IsRoot() && */ !childProviderObject.HasContainer())
            {
                SetContainer(parentProviderObject, childProviderObject);
            }
        }
    }

    /// <summary>
    /// Sets the container information of the childObject to be associated to the parentObject
    /// </summary>
    /// <param name="parentObject">Object, who will be the parent of the child object</param>
    /// <param name="childObject">Child object getting the parent object as container</param>
    private static void SetContainer(IProviderObject parentObject, IProviderObject childObject)
    {
#if DEBUG
        if (parentObject.Equals(childObject))
        {
            Debugger.Break();
            Debug.Fail("parentObject == childObject");
        }

        // Check by recursion
        var parentContainer = parentObject;
        for (var n = 0; n < 1000; n++)
        {
            parentContainer = parentContainer.GetContainer();

            if (parentContainer == null)
            {
                break;
            }
            
            if (parentContainer.Equals(childObject))
            {
                Debugger.Break();
                Debug.Fail("parentObject == childObject");
            }

            if (n == 999)
            {
                Debugger.Break();
                Debug.Fail("Unlimited recursion");
            }
        }
#endif
        childObject.SetContainer(parentObject);
    }

    /// <inheritdoc />
    public virtual bool isSet(string property)
        => ProviderObject.IsPropertySet(property);

    /// <inheritdoc />
    public void unset(string property)
    {
        ProviderObject.DeleteProperty(property);
            
        _extent?.ChangeEventManager?.SendChangeEvent(this);
    }

    /// <inheritdoc />
    public IEnumerable<string> getPropertiesBeingSet()
        => ProviderObject.GetProperties();

#if DEBUG
    private int _stackDepth;
#endif
    /// <inheritdoc />
    public override string ToString()
    {
#if DEBUG

        Interlocked.Increment(ref _stackDepth);
        if (_stackDepth > 50)
        {
            Debugger.Break();
            return "STACKOVERFLOW";
        }

        var result = NamedElementMethods.GetName(this, true);

        Interlocked.Decrement(ref _stackDepth);

        return result;
#else
            return NamedElementMethods.GetName(this, true);
#endif
    }

    /// <summary>
    /// Gets the given object as a meta object
    /// </summary>
    /// <returns>This element itself</returns>
    public MofObject GetMetaObject() => this;

    public IObject CreatedBy(MofExtent extent)
    {
        ReferencedExtent = extent ?? throw new ArgumentNullException(nameof(extent));
        return this;
    }

    internal void UpdateContent()
    {
        _extent?.ChangeEventManager?.SendChangeEvent(this);
        _extent?.SignalUpdateOfContent();
    }
}

public class MofObjectEqualityComparer : IEqualityComparer<IObject?>
{
    public bool Equals(IObject? x, IObject? y)
    {
        var result = MofObject.AreEqual(x, y);
        return result;
    }

    public int GetHashCode(IObject? obj)
    {
        return obj?.GetHashCode() ?? 0;
    }
}

public class MofElementEqualityComparer : IEqualityComparer<IElement?>
{
    public bool Equals(IElement? x, IElement? y)
    {
        var result = MofObject.AreEqual(x, y);
        return result;
    }

    public int GetHashCode(IElement? obj)
    {
        return obj?.GetHashCode() ?? 0;
    }
}