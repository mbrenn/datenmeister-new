using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.InMemory;

namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Provides predefined copy option configurations for common copying scenarios.
/// </summary>
public static class CopyOptions
{
    /// <summary>
    /// Gets the default copy options which create new ids for each copied element.
    /// This is the standard behavior for creating independent copies.
    /// </summary>
    public static CopyOption None { get; } = new();

    /// <summary>
    /// Gets the copy options which also copies the ids from source to target elements.
    /// Use this when preserving original identifiers is required.
    /// </summary>
    public static CopyOption CopyId => new() {CopyId = true};
}

/// <summary>
/// Encapsulates the parameters passed to the PredicateToClone function in CopyOption.
/// Used to provide context information for determining whether an object should be cloned.
/// </summary>
public struct CopyParameters
{
    /// <summary>
    /// Gets or sets the source object from which the property is being copied.
    /// </summary>
    public object SourceObject { get; set; }

    /// <summary>
    /// Gets or sets the object that is being evaluated for copying.
    /// </summary>
    public object ObjectToBeCopied { get; set; }

    /// <summary>
    /// Gets or sets the name of the property being copied.
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the target object that will receive the copied property.
    /// </summary>
    public IObject TargetObject { get; set; }
}

/// <summary>
/// Defines options that control the behavior of the object copying process.
/// These options allow fine-grained control over ID handling, reference cloning,
/// recursion depth, and custom copy predicates.
/// </summary>
public class CopyOption
{
    /// <summary>
    /// Gets or sets a value indicating whether the IDs of the objects shall be copied
    /// or whether a new ID shall be generated for each copied element.
    /// Default is false (new IDs are generated).
    /// </summary>
    public bool CopyId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all references shall be cloned,
    /// preventing the creation of URI references to external extents.
    /// When true, all referenced objects are copied rather than referenced.
    /// Default is false.
    /// </summary>
    public bool CloneAllReferences { get; set; }

    /// <summary>
    /// Gets or sets the predicate function that determines whether a given object should be cloned
    /// during the copy process. The predicate is evaluated based on specific parameters such as the
    /// source object, property name, and target object. When this predicate returns true,
    /// the object will be forcefully copied regardless of other options.
    /// </summary>
    public Predicate<CopyParameters>? PredicateToClone { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only primitive elements shall be copied
    /// without any recursive copying of nested objects. When true, complex nested structures
    /// are not traversed. Default is false.
    /// </summary>
    public bool NoRecursion { get; set; }
}

/// <summary>
/// Provides functionality to copy MOF (Meta Object Facility) elements and their properties.
/// The ObjectCopier handles deep copying of objects, including nested structures, collections,
/// and cross-extent references. It supports configurable behavior through CopyOption settings.
/// </summary>
public class ObjectCopier
{
    /// <summary>
    /// Defines the maximum recursion depth that is accepted by the object copier.
    /// This prevents infinite recursion in case of circular references.
    /// </summary>
    private const int MaxRecursionDepth = 100;

    /// <summary>
    /// The factory used to create new instances of MOF elements during the copy process.
    /// </summary>
    private readonly IFactory _factory;

    /// <summary>
    /// Tracks the current recursion depth during the copy operation
    /// to prevent exceeding MaxRecursionDepth.
    /// </summary>
    private int _currentDepth;

    /// <summary>
    /// Stores the extent of the source element being copied.
    /// This information is used to determine whether an element shall be deep copied
    /// or referenced via URI. Property values referencing external extents are not copied;
    /// instead, URI references are preserved.
    /// </summary>
    private IExtent? _sourceExtent;

    /// <summary>
    /// Initializes a new instance of the ObjectCopier class.
    /// </summary>
    /// <param name="factory">The factory used to create new MOF elements during copying.</param>
    /// <exception cref="ArgumentNullException">Thrown when factory is null.</exception>
    public ObjectCopier(IFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Creates a deep copy of the specified MOF element, including all its properties.
    /// The method creates a new instance with the same metaclass and recursively copies
    /// all property values according to the provided copy options.
    /// </summary>
    /// <param name="element">The MOF element to be copied.</param>
    /// <param name="copyOptions">Options controlling the copy behavior. If null, default options are used.</param>
    /// <returns>A new IElement instance containing the copied data.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the element has no associated extent.</exception>
    public IElement Copy(IObject element, CopyOption? copyOptions = null)
    {
        copyOptions ??= CopyOptions.None;

        // Gets the source extent
        _sourceExtent = (element as IHasExtent)?.Extent ?? (element as MofElement)?.ReferencedExtent
            ?? throw new InvalidOperationException("element is not IHasExtent and not MofElement");

        IElement targetElement;
        if (element is MofObject {IsSlimUmlEvaluation: true} asObject)
        {
            targetElement = _factory.create(
                asObject.ProviderObject.MetaclassUri == null
                    ? null
                    : new MofObjectShadow(asObject.ProviderObject.MetaclassUri));
        }
        else
        {
            targetElement = _factory.create((element as IElement)?.getMetaClass());
        }

        CopyProperties(element, targetElement, copyOptions);

        return targetElement;
    }

    /// <summary>
    /// Copies all properties from a source element to a target element.
    /// This method recursively copies properties based on the provided copy options,
    /// handling IDs, nested objects, and collections. The recursion depth is tracked
    /// to prevent stack overflow in case of circular references.
    /// </summary>
    /// <param name="sourceElement">The source element from which properties are copied.</param>
    /// <param name="targetElement">The target element to which properties are copied.</param>
    /// <param name="copyOptions">Options controlling the copy behavior. If null, default options are used.</param>
    /// <exception cref="ArgumentNullException">Thrown when sourceElement or targetElement is null.</exception>
    /// <exception cref="ArgumentException">Thrown when sourceElement does not implement IObjectAllProperties.</exception>
    public void CopyProperties(IObject sourceElement, IObject targetElement, CopyOption? copyOptions = null)
    {
        _currentDepth++;
        if (_currentDepth >= MaxRecursionDepth)
        {
            // Can't go deeper
            return;
        }

        copyOptions ??= CopyOptions.None;

        ArgumentNullException.ThrowIfNull(sourceElement);
        ArgumentNullException.ThrowIfNull(targetElement);
        if (sourceElement is not IObjectAllProperties elementAsExt)
        {
            throw new ArgumentException($"{nameof(sourceElement)} is not of type IObjectAllProperties");
        }

        // Transfers the id, if requested by the copy options
        if (copyOptions.CopyId
            && sourceElement is IHasId sourceWithId
            && targetElement is ICanSetId targetCanSetId)
        {
            var id = sourceWithId.Id;
            if (id != null)
            {
                targetCanSetId.Id = id;
            }
        }

        // Transfers the properties
        foreach (var property in elementAsExt.getPropertiesBeingSet())
        {
            var value = sourceElement.get<object>(property, !copyOptions.CloneAllReferences);
            
            var forceCopy = false;
            if (copyOptions.PredicateToClone != null)
            {
                var parameters = new CopyParameters
                {
                    ObjectToBeCopied = value,
                    PropertyName = property,
                    TargetObject = targetElement
                };
                
                forceCopy = copyOptions.PredicateToClone(parameters);
            }
            
            var result = CopyValue(value, copyOptions, forceCopy);
            targetElement.set(property, result);
        }

        _currentDepth--;
    }

    /// <summary>
    /// Copies a single property value, determining whether to create a deep copy,
    /// maintain a reference, or return the value as-is based on its type and copy options.
    /// This method handles different value types including null, elements, collections, and primitives.
    /// </summary>
    /// <param name="value">The property value to be copied.</param>
    /// <param name="copyOptions">Copy options controlling the behavior. If null, default options are used.</param>
    /// <param name="forceCopy">When true, forces a deep copy regardless of extent relationships or other copy options.</param>
    /// <returns>The copied value, which may be a new instance, a reference, or the original value depending on type and options.</returns>
    private object? CopyValue(object? value, CopyOption? copyOptions = null, bool forceCopy = false)
    {
        copyOptions ??= CopyOptions.None;
        var noRecursion = copyOptions.NoRecursion;

        switch (value)
        {
            case null:
            case IElement _ when noRecursion:
                return null;

            case MofObjectShadow asMofObjectShadow:
                return asMofObjectShadow;

            case IElement valueAsElement:
            {
                var propertyExtent = (valueAsElement as IHasExtent)?.Extent;
                
                // We will do a full copy, if required
                var doCopy = forceCopy;
                
                // 1) The extent in which the property itself relies is null
                doCopy |= propertyExtent == null;
                
                // 2) The value is copied within the same extent
                doCopy |= propertyExtent == _sourceExtent;
                
                // 3) If the flag CloneAllReference is set
                doCopy |= copyOptions.CloneAllReferences;
                
                return doCopy 
                    ? Copy(valueAsElement, copyOptions) // It was decided to copy 
                    : value;
            }
            case IReflectiveCollection _ when noRecursion:
                return null;
            case IReflectiveCollection valueAsCollection:
                return valueAsCollection
                    .Select(innerValue => CopyValue(innerValue, copyOptions, forceCopy));
            default:
                return value;
        }
    }

    /// <summary>
    /// Static helper method to create a copy of a MOF element using the specified factory.
    /// This is a convenience method that creates an ObjectCopier instance and performs the copy operation.
    /// </summary>
    /// <param name="factory">The factory used to create new MOF elements during copying.</param>
    /// <param name="element">The MOF element to be copied.</param>
    /// <param name="copyOptions">Options controlling the copy behavior. If null, default options are used.</param>
    /// <returns>A new IElement instance containing the copied data.</returns>
    public static IElement Copy(IFactory factory, IObject element, CopyOption? copyOptions = null)
    {
        copyOptions ??= CopyOptions.None;

        var copier = new ObjectCopier(factory);
        return copier.Copy(element, copyOptions);
    }

    /// <summary>
    /// Static helper method to copy properties from a source element to an existing target element.
    /// This method creates a temporary ObjectCopier using a factory derived from the target element
    /// and copies all properties from source to target.
    /// </summary>
    /// <param name="source">The source element from which properties are copied.</param>
    /// <param name="target">The existing target element to which properties are copied.</param>
    /// <param name="copyOptions">Options controlling the copy behavior. If null, default options are used.</param>
    public static void CopyPropertiesStatic(IObject source, IObject target, CopyOption? copyOptions = null)
    {
        copyOptions ??= CopyOptions.None;

        var copier = new ObjectCopier(new MofFactory(target));
        copier.CopyProperties(source, target, copyOptions);
    }

    /// <summary>
    /// Creates a temporary copy of a MOF element using the in-memory provider.
    /// The copied element is stored in a temporary extent and is suitable for transient operations.
    /// This method also transfers meta-extents from the source to the temporary extent.
    /// </summary>
    /// <param name="value">The MOF element to be copied.</param>
    /// <returns>A temporary IObject instance stored in the in-memory temporary extent.</returns>
    public static IObject CopyForTemporary(IObject value)
    {
        if (value is MofObject mofObject)
        {
            InMemoryProvider.TemporaryExtent.AddMetaExtents((mofObject).ReferencedExtent.MetaExtents);
        }

        // Adds the data workspaces
        return Copy(InMemoryObject.TemporaryFactory, value, CopyOptions.None);
    }

    /// <summary>
    /// Creates a temporary copy of a reflective collection using the in-memory provider.
    /// Each element in the collection is copied to the temporary extent. The resulting collection
    /// is suitable for transient operations and modifications that should not affect the original data.
    /// </summary>
    /// <param name="value">The reflective collection to be copied.</param>
    /// <param name="copyOptions">Options controlling the copy behavior for each element. If null, default options are used.</param>
    /// <returns>A temporary IReflectiveCollection containing copies of all elements from the source collection.</returns>
    public static IReflectiveCollection CopyForTemporary(IReflectiveCollection value,
        CopyOption? copyOptions = null)
    {
        var temp = new TemporaryReflectiveCollection();
        copyOptions ??= CopyOptions.None;

        foreach (var item in value.OfType<IObject>())
        {
            temp.add(Copy(InMemoryObject.TemporaryFactory, item, copyOptions));
        }

        return temp;
    }
}