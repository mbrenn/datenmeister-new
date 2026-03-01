using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.InMemory;

namespace DatenMeister.Core.Runtime.Copier;

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
    /// Tracks the number of times the CopyValue method has been called.
    /// Used for performance monitoring and instrumentation.
    /// Thread-safe counter using Interlocked operations.
    /// </summary>
    private static long _copyValueCallCount;

    /// <summary>
    /// Gets the number of times the CopyValue method has been called.
    /// </summary>
    public static long CopyValueCallCount => _copyValueCallCount;

    /// <summary>
    /// Tracks the number of times the PredicateToClone in CopyOption has been invoked.
    /// Used for performance monitoring and instrumentation.
    /// Thread-safe counter using Interlocked operations.
    /// </summary>
    private static long _predicateInvocationCount;

    /// <summary>
    /// Gets the number of times the PredicateToClone in CopyOption has been invoked.
    /// </summary>
    public static long PredicateInvocationCount => _predicateInvocationCount;

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
    /// </summary>
    /// <param name="sourceElement">Element from which the properties shall be taken</param>
    /// <param name="targetElement">Element to which the properties shall be put to </param>
    /// <param name="copyOptions">The options for copying. In case of null, the default is used</param>
    public void CopyProperties(IObject sourceElement, IObject targetElement, CopyOption? copyOptions = null)
    {
        InternalCopyProperties(sourceElement, targetElement, copyOptions);
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
    private void InternalCopyProperties(IObject sourceElement, IObject targetElement, CopyOption? copyOptions = null)
    {
        _currentDepth++;
        if (_currentDepth >= MaxRecursionDepth)
        {
            // Can't go deeper
            throw new InvalidOperationException("Max recursion depth reached");
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
            if (!sourceElement.isSet(property))
            {
                // Element is not set or has its default value,so skip it. 
                continue;
            }
            
            var value = sourceElement.get<object>(property, !copyOptions.CloneAllReferences);
            
            var forceCopy = CopyType.Undefined;
            if (copyOptions.PredicateToClone != null)
            {
                var parameters = new CopyParameters
                {
                    SourceObject = sourceElement,
                    ObjectToBeCopied = value,
                    PropertyName = property,
                    TargetObject = targetElement
                };

                Interlocked.Increment(ref _predicateInvocationCount);
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
    /// <param name="copyType">When true, forces a deep copy regardless of extent relationships or other copy options.</param>
    /// <returns>The copied value, which may be a new instance, a reference, or the original value depending on type and options.</returns>
    private object? CopyValue(object? value, CopyOption? copyOptions = null, CopyType copyType = CopyType.KeepReference)
    {
        Interlocked.Increment(ref _copyValueCallCount);

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
                
                // 1) The extent in which the property itself relies is null
                if (copyType == CopyType.Undefined && propertyExtent == null) 
                    copyType = CopyType.Clone;
                
                // 2) The value is copied within the same extent
                if (copyType == CopyType.Undefined 
                    && propertyExtent == _sourceExtent && MofExtent.GlobalSlimUmlEvaluation) 
                    copyType = CopyType.Clone;

                // 3) If the flag CloneAllReference is set
                if(copyOptions.CloneAllReferences)
                    copyType = CopyType.Clone;
                
                return copyType == CopyType.Clone 
                    ? Copy(valueAsElement, copyOptions) // It was decided to copy 
                    : value;
            }
            case IReflectiveCollection _ when noRecursion:
                return null;
            case IReflectiveCollection valueAsCollection:
                return valueAsCollection
                    .Select(innerValue => CopyValue(innerValue, copyOptions, 
                        copyType));
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