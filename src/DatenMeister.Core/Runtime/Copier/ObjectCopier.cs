using System.Diagnostics;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider;
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

    public IElement InternalCopy(IObject element, CopyOption? copyOptions)
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
        
        var uri = element.GetUri();
        if (!string.IsNullOrEmpty(uri))
        {
            _cloneDictionary[CleanUri(uri)] = targetElement;
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
    public void ExecutePreCopyActions()
    {
        if (_isActive)
            throw new InvalidOperationException("Copying is requested while another copying is active");

        _isActive = true;
        _cloneDictionary.Clear();
    }

    /// <summary>
    /// Executes all post-copy actions that have been queued during the copy process.
    /// </summary>
    public void ExecutePostCopyActions()
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

            var value = sourceElement.get<object>(property, true);
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

                if (value is UriReference && forceCopy == CopyType.Clone)
                {
                    value = sourceElement.get<object>(property);
                }
            }

            var result = InternalCopyValue(value, copyOptions, forceCopy,
                FullDebug
                    ? ((sourceElement as IElement)?.getMetaClass()?.ToString() ?? "Unknown") + "." + property
                    : "");
            if (result.CopyType == CopyType.FindClonedReference)
            {
                var indirectResult = result.IndirectResult;
                if (indirectResult == null)
                {
                    throw new InvalidOperationException("IndirectResult is null");
                }

                _postCopyActions.Add(() => { targetElement.set(property, indirectResult()); });
            }
            else
            {
                targetElement.set(property, result.DirectResult);
            }
        }

        _currentDepth--;
    }

    /// <summary>
    /// Logs a trace message for CopyType operations when FullDebug is enabled.
    /// The message is indented based on the current recursion depth and formatted with fixed column widths.
    /// </summary>
    /// <param name="copyType">The type of copy operation being performed.</param>
    /// <param name="value">The value being copied.</param>
    /// <param name="typeLabel">Optional label to identify the type of value (e.g., "UriReference", "IElement").</param>
    /// <param name="propertyName">Optional property name being copied.</param>
    private void TraceCopyType(CopyType copyType, object? value, string? typeLabel = null, string? propertyName = null)
    {
        if (!FullDebug) return;

        var indent = new string(' ', _currentDepth * 2);
        var copyTypeStr = copyType.ToString().Length > 10 ? copyType.ToString()[..10] : copyType.ToString().PadRight(10);
        var typeStr = (typeLabel ?? "").Length > 10 ? (typeLabel ?? "")[..10] : (typeLabel ?? "").PadRight(10);
        var propStr = (propertyName ?? "").Length > 30 ? (propertyName ?? "")[..30] : (propertyName ?? "").PadRight(30);
        var valueStr = 
            value is IHasId asHasId && asHasId.Id != null 
                ? $"{value}[{asHasId.Id}]" 
                : value?.ToString() ?? "null";

        Logger.Trace($"{copyTypeStr}|{typeStr}|{propStr}:{indent}{valueStr}");
    }

    /// <summary>
    /// Copies a single property value, determining whether to create a deep copy,
    /// maintain a reference, or return the value as-is based on its type and copy options.
    /// This method handles different value types including null, elements, collections, and primitives.
    /// </summary>
    /// <param name="value">The property value to be copied.</param>
    /// <param name="copyOptions">Copy options controlling the behavior. If null, default options are used.</param>
    /// <param name="copyType">When true, forces a deep copy regardless of extent relationships or other copy options.</param>
    /// <param name="propertyName">Optional name of the property being copied, used for tracing.</param>
    /// <returns>The copied value, which may be a new instance, a reference, or the original value depending on type and options.</returns>
    private CopyResult InternalCopyValue(object? value, CopyOption? copyOptions, CopyType copyType, string? propertyName = null)
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
            case UriReference uriReference:
                TraceCopyType(copyType, value, "UriRef", propertyName);
                switch (copyType)
                {
                    case CopyType.Undefined:
                        throw new InvalidOperationException("Copy type is undefined, but should be defined by now");
                    case CopyType.Clone:
                        throw new InvalidOperationException("UriReference cannot be cloned (at least now)");
                    case CopyType.KeepReference:
                        return CopyResult.CreateResultForReference(value);
                    case CopyType.FindClonedReference:
                        var referenceUri = uriReference.Uri;
                        if (!string.IsNullOrEmpty(referenceUri))
                        {
                            return CopyResult.CreateResultForToFindClonedReference(() =>
                            {
                                if (_cloneDictionary.TryGetValue(CleanUri(referenceUri), out var reference))
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Adding reference for: {referenceUri}");
                                    return reference;
                                }
                                else
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Did not find: {referenceUri}");
                                    return null;
                                }
                            });
                        }
                        
                        return CopyResult.CreateResultForInstance(value);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(copyType), copyType, null);
                }

            case IElement valueAsElement:
            {
                var propertyExtent = (valueAsElement as IHasExtent)?.Extent;

                // 1) The extent in which the property itself relies is not existing
                if (copyType == CopyType.Undefined && propertyExtent == null)
                    copyType = CopyType.Clone;

                // 2) The value is copied within the same extent
                if (copyType == CopyType.Undefined
                    && propertyExtent == _sourceExtent
                    && MofExtent.GlobalSlimUmlEvaluation)
                    copyType = CopyType.Clone;

                TraceCopyType(copyType, value, "IElement", propertyName);

                switch (copyType)
                {
                    case CopyType.Undefined:
                        throw new InvalidOperationException("Copy type is undefined, but should be defined by now");
                    case CopyType.Clone:
                        var result = InternalCopy(valueAsElement, copyOptions);
                        return CopyResult.CreateResultForInstance(result);
                    case CopyType.KeepReference:
                        return CopyResult.CreateResultForReference(value);
                    case CopyType.FindClonedReference:
                        var referenceUri = valueAsElement.GetUri();
                        if (!string.IsNullOrEmpty(referenceUri))
                        {
                            return CopyResult.CreateResultForToFindClonedReference(() =>
                            {
                                if (_cloneDictionary.TryGetValue(CleanUri(referenceUri), out var reference))
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Adding reference for: {referenceUri}");
                                    return reference;
                                }
                                else
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Did not find: {referenceUri}");
                                    return null;
                                }
                            });
                        }
                        
                        return CopyResult.CreateResultForInstance(value);
                    default: 
                        throw new ArgumentOutOfRangeException(nameof(copyType), copyType, null);
                }
            }
            case IReflectiveCollection _ when noRecursion:
                return CopyResult.CreateResultForInstance(value);
            case IReflectiveCollection valueAsCollection:
                TraceCopyType(copyType, value, "IReflColl", propertyName);
                if (copyType == CopyType.FindClonedReference)
                {
                    var uris = 
                        valueAsCollection.Select(innerValue =>
                        {
                            if (innerValue is IObject asObject)
                            {
                                var uri = asObject.GetUri();
                                return string.IsNullOrEmpty(uri)? null : new UriReference(uri);
                            }

                            return innerValue;

                        }).ToList();
                    
                    return CopyResult.CreateResultForToFindClonedReference(() =>
                    {
                        var listResult = new List<object?>();
                        foreach (var innerValue in uris)
                        {
                            if (innerValue is UriReference uriReference)
                            {
                                var referenceUri = uriReference.Uri;
                                if (string.IsNullOrEmpty(referenceUri))
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Having a null");
                                    listResult.Add(null);
                                }
                                else if (_cloneDictionary.TryGetValue(CleanUri(referenceUri), out var reference))
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Adding reference for: {referenceUri}");
                                    listResult.Add(reference);
                                }
                                else
                                {
                                    if (FullDebug)
                                        Logger.Trace($"PostCopyAction: Did not find: {referenceUri}");
                                    return null;
                                }
                            }
                            else
                            {
                                if (FullDebug)
                                    Logger.Trace($"PostCopyAction: Adding value: {innerValue}");
                                listResult.Add(innerValue);
                            }
                        }

                        return listResult;

                    });
                }
                else
                {
                    var listResult = valueAsCollection.Select(innerValue =>
                    {
                        var innerResult = InternalCopyValue(innerValue, copyOptions, copyType, propertyName);
                        if (innerResult.CopyType != CopyType.Clone && innerResult.CopyType != CopyType.KeepReference)
                        {
                            throw new InvalidOperationException("Something obscure happened");
                        }

                        return innerResult.DirectResult;
                    }).ToList();

                    return CopyResult.CreateResultForInstance(listResult);

                }

            default:
                return CopyResult.CreateResultForInstance(value);
        }
    }

    /// <summary>
    /// Cleans up ur by just considering the part of the uri after the hash '#'.
    /// This allows being independent of the absolute or relative links
    /// </summary>
    /// <param name="cleanUri">Uri to be cleaned</param>
    /// <returns></returns>
    private static string CleanUri(string cleanUri)
    {
        var hashIndex = cleanUri.IndexOf('#');
        return hashIndex >= 0 ? cleanUri.Substring(hashIndex + 1) : cleanUri;
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