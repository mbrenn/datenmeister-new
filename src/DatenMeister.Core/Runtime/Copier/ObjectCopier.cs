using BurnSystems.Logging;
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
    /// Defines the logger
    /// </summary>
    private static ILogger Logger = new ClassLogger(typeof(ObjectCopier));

    /// <summary>
    /// Stores a flag which activates the full debugging of each copying by adding information
    /// via the classlogger. This helps to figure out issues in the debugging
    /// </summary>
    public static bool FullDebug { get; set; } = false;

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
    /// Gets or sets a flag indicating, that the copying is active
    /// This is set by the PreCopyActions and reset by the PostCopyActions
    /// In case a copying is started while it is already active, an exception is being thrown
    /// </summary>
    private bool _isActive;

    /// <summary>
    /// Stores a dictionary of cloned items. The key is the old uri of the element.
    /// The value is the cloned element. 
    /// </summary>
    private readonly Dictionary<string, IElement> _cloneDictionary = new();

    /// <summary>
    /// These actions are performed after the full copy has been done
    /// </summary>
    private readonly List<Action> _postCopyActions = new();

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
        ExecutePreCopyActions();
        
        var targetElement = InternalCopy(element, copyOptions);

        ExecutePostCopyActions();
        return targetElement;
    }

    private IElement InternalCopy(IObject element, CopyOption? copyOptions)
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

        InternalCopyProperties(element, targetElement, copyOptions);
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
        ExecutePreCopyActions();
        InternalCopyProperties(sourceElement, targetElement, copyOptions);
        ExecutePostCopyActions();
    }

    /// <summary>
    /// Executes all necessary actions that have to be done before the
    /// copying is started
    /// </summary>
    private void ExecutePreCopyActions()
    {
        if (_isActive)
            throw new InvalidOperationException("Copying is requested while another copying is active");

        _isActive = true;
        _cloneDictionary.Clear();
    }

    /// <summary>
    /// Executes all post-copy actions that have been queued during the copy process.
    /// </summary>
    private void ExecutePostCopyActions()
    {
        foreach (var action in _postCopyActions)
        {
            action();
        }

        _postCopyActions.Clear();
        _isActive = false;
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
            
            var result = InternalCopyValue(value, copyOptions, forceCopy);
            if (result.CopyType == CopyType.KeepReference)
            {
                _postCopyActions.Add(() =>
                {
                    if (result.IndirectResult == null)
                    {
                        throw new InvalidOperationException("IndirectResult is null");
                    }
                    
                    targetElement.set(property, result.IndirectResult());
                });
            }
            else
            {
                targetElement.set(property, result.DirectResult);
            }
        }

        _currentDepth--;
    }

    /// <summary>
    /// Defines the structure that is used to return information of the copying
    /// </summary>
    public struct CopyResult
    {
        /// <summary>
        /// Defines the type of the copy that had been executed
        /// </summary>
        public CopyType CopyType;

        /// <summary>
        /// Returns the direct result in case there is no CopyType.FindClonedReference returned
        /// </summary>
        public object? DirectResult;

        /// <summary>
        /// Returns the indirect result in case there is a CopyType.FindClonedReference returned
        /// </summary>
        public Func<object?>? IndirectResult;

        public static CopyResult CreateResultForInstance(object? result)
        {
            return new CopyResult
            {
                CopyType = CopyType.Clone,
                DirectResult = result
            };
        }
        
        public static CopyResult CreateResultForReference(object? reference)
        {
            return new CopyResult
            {
                CopyType = CopyType.FindClonedReference,
                DirectResult = reference
            };
        }

        public static CopyResult CreateResultForKeepReference(Func<object?> reference)
        {
            return new CopyResult
            {
                CopyType = CopyType.KeepReference,
                IndirectResult = reference
            };
        }
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
    private CopyResult InternalCopyValue(object? value, CopyOption? copyOptions, CopyType copyType)
    {
        Interlocked.Increment(ref _copyValueCallCount);

        copyOptions ??= CopyOptions.None;
        var noRecursion = copyOptions.NoRecursion;

        switch (value)
        {
            case null:
            case IElement _ when noRecursion:
                return CopyResult.CreateResultForInstance(null);

            case MofObjectShadow asMofObjectShadow:
                return CopyResult.CreateResultForInstance(asMofObjectShadow);

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
                if (copyOptions.CloneAllReferences)
                    copyType = CopyType.Clone;

                if (FullDebug)
                {
                    Logger.Trace($"{value?.ToString() ?? "Unknown"} copied via {copyType}");
                }

                switch (copyType)
                {
                    case CopyType.Undefined:
                        throw new InvalidOperationException
                            ("Copy type is undefined, but should be defined by now");
                    case CopyType.Clone:
                        var result = InternalCopy(valueAsElement, copyOptions);
                        var uri = valueAsElement.GetUri();
                        if (!string.IsNullOrEmpty(uri))
                        {
                            _cloneDictionary[uri] = result;
                        }

                        return CopyResult.CreateResultForInstance(result);
                    case CopyType.KeepReference:
                        return CopyResult.CreateResultForReference(value);
                    case CopyType.FindClonedReference:
                        var referenceUri = valueAsElement.GetUri();
                        if (!string.IsNullOrEmpty(referenceUri))
                        {
                            return CopyResult.CreateResultForKeepReference(() =>
                            {
                                if (_cloneDictionary.TryGetValue(referenceUri, out var reference))
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Adding reference for: ${referenceUri}");
                                    return reference;
                                }
                                else
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Did not find: ${referenceUri}");

                                    return null;
                                }
                            });
                        }
                        
                        return CopyResult.CreateResultForInstance(value);
                }

                throw new ArgumentOutOfRangeException(nameof(copyType), copyType, null);

            }
            case IReflectiveCollection _ when noRecursion:
                return CopyResult.CreateResultForInstance(value);
            case IReflectiveCollection valueAsCollection:
                var copyTypeCopy = copyType;
                return CopyResult.CreateResultForInstance(
                    valueAsCollection.Select(innerValue =>
                    {
                        var result = InternalCopyValue(innerValue, copyOptions, copyTypeCopy);
                        if (result.CopyType == CopyType.KeepReference)
                        {
                            return null;
                        }
                        else
                        {
                            return result.DirectResult;
                        }
                    }));
            default:
                return CopyResult.CreateResultForInstance(value);
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