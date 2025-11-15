using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.Core.TypeIndexAssembly.Model;

// ReSharper disable HeuristicUnreachableCode

#pragma warning disable CS0162 // Unreachable code detected (Compile-Time Configurations)

namespace DatenMeister.Core.Provider.InMemory;

internal enum IndexCache
{
    None,
    Index,
    Instance
}
    
/// <summary>
/// Stores all elements in the memory
/// </summary>
public class InMemoryProvider : IProvider, ICanUpdateCacheId
{
    /// <summary>
    /// Defines a configuration variable whether the index cache itself shall be used.
    /// It might speed up the finding and deletion of items
    /// </summary>
    private const IndexCache ConfigUseIndexCache = IndexCache.Instance;
        
    /// <summary>
    /// Stores the temporary extent that can be used to create temporary objects
    /// </summary>
    public static readonly MofUriExtent TemporaryExtent = new(new InMemoryProvider(), "dm:///temp", null);

    /// <summary>
    /// Stores the elements of the current provider
    /// </summary>
    private readonly List<InMemoryObject> _elements = new();

    /// <summary>
    /// Stores the memory object for local information
    /// </summary>
    private readonly InMemoryObject _extentElement;

    /// <summary>
    /// Stores the index for the object
    /// </summary>
    private readonly Dictionary<string, int> _objectIndex = new();
        
    /// <summary>
    /// Stores the instances for the objects within the cache
    /// </summary>
    private readonly Dictionary<string, InMemoryObject> _instanceCache = new();
    
        
    /// <summary>
    ///     Stores the capabilities of the provider
    /// </summary>
    /// <returns></returns>
    private readonly ProviderCapability _providerCapability = new()
    {
        IsTemporaryStorage = true
    };

    public InMemoryProvider()
    {
        _extentElement = new InMemoryObject(this);
    }

    /// <summary>
    ///     Gets the used temporary provider
    /// </summary>
    public static IProvider TemporaryProvider => (InMemoryProvider) TemporaryExtent.Provider;
    
    /// <summary>
    /// Stores the context to retrieve information about types from the typeindex logic.
    /// </summary>
    internal TypeIndexInWorkspaceContext? TypeIndex { get; set; }

    /// <inheritdoc />
    public IProviderObject CreateElement(string? metaClassUri) =>
        new InMemoryObject(this, metaClassUri);

    /// <inheritdoc />
    public void AddElement(IProviderObject? valueAsObject, int index = -1)
    {
        if (valueAsObject == null)
            return; // Wo do not add empty elements

        lock (_elements)
        {
            var toBeAdded = (InMemoryObject) valueAsObject;
            if (index == -1)
            {
                _elements.Add(toBeAdded);
            }
            else
            {
                _elements.Insert(index, toBeAdded);
            }

            if (ConfigUseIndexCache == IndexCache.Instance)
            {
                var id = toBeAdded.Id;
                if (id != null)
                {
                    _instanceCache[id] = toBeAdded;
                }
            }
        }
    }

    /// <summary>
    /// Gets the index of the element within the storage.
    /// In case it is not found, it will reiterate the list and rebuild the index
    /// </summary>
    /// <param name="id">Id of the element to be found</param>
    /// <returns>Index or -1, if element is not found</returns>
    private int GetIndexOfElement(string id)
    {
        lock (_elements)
        {
            switch (ConfigUseIndexCache)
            {
                case IndexCache.Index:
                {
                    if (_objectIndex.TryGetValue(id, out var index))
                    {
                        // Checks that the value is still fitting
                        if (index < _elements.Count && _elements[index].Id == id)
                        {
                            return index;
                        }
                    }

                    var result = -1;

                    // Rebuild the index
                    _objectIndex.Clear();
                    for (var i = 0; i < _elements.Count; i++)
                    {
                        var element = _elements[i];

                        var idOfElement = element.Id;
                        if (!string.IsNullOrEmpty(idOfElement))
                        {
                            _objectIndex[idOfElement] = i;
                            if (idOfElement == id)
                            {
                                result = i;
                            }
                        }
                    }

                    return result;
                }
                
                default:
                    throw new InvalidOperationException(
                        "This function should never be called configuration with ConfigUseIndexCache = false");
            }
        }
    }

    
    /// <inheritdoc />
    public bool DeleteElement(string id)
    {
        lock (_elements)
        {
            switch (ConfigUseIndexCache)
            {
                case IndexCache.Index:
                {
                    // Gets the index of the element
                    var index = GetIndexOfElement(id);
                    if (index != -1)
                    {
                        _elements.RemoveAt(index);
                        return true;
                    }

                    return false;
                }
                case IndexCache.None:
                    return _elements.RemoveAll(x => x.Id == id) > 0;
                case IndexCache.Instance:
                    if (_instanceCache.Remove(id, out var element))
                    {
                        return _elements.Remove(element);
                    }

                    return false;
            }
        }
    }

    /// <inheritdoc />
    public void DeleteAllElements()
    {
        lock (_elements)
        {
            _elements.Clear();
                
            if (ConfigUseIndexCache == IndexCache.Instance)
            {
                _instanceCache.Clear();
            }
        }
    }

    /// <inheritdoc />
    public IProviderObject? Get(string? id)
    {
        lock (_elements)
        {
            switch (ConfigUseIndexCache)
            {
                case IndexCache.Index when id == null:
                    return _extentElement;
                case IndexCache.Index:
                {
                    var index = GetIndexOfElement(id);
                    return index == -1 ? null : _elements[index];
                }
                case IndexCache.None:
                    return _elements.FirstOrDefault(x => x.Id == id);
                case IndexCache.Instance when id == null:
                    return null;
                case IndexCache.Instance:
                    return _instanceCache.GetValueOrDefault(id);
            }
        }
    }

    /// <inheritdoc />
    public IEnumerable<IProviderObject> GetRootObjects()
    {
        lock (_elements)
        {
            return _elements.ToList();
        }
    }

    /// <summary>
    /// Gets the capabilities of the provider
    /// </summary>
    /// <returns></returns>
    public ProviderCapability GetCapabilities() => _providerCapability;

    public void Lock()
    {
        Monitor.Enter(_elements);
    }

    public void Unlock()
    {
        Monitor.Exit(_elements);
    }

    /// <summary>
    /// Gets the information that the id of an element has changed.
    /// This will lead to an update of the internal caches
    /// </summary>
    /// <param name="inMemoryObject">Object which has been modified</param>
    /// <param name="formerId">Former Id of the element before the modification</param>
    void ICanUpdateCacheId.UpdateCachedId(InMemoryObject inMemoryObject, string? formerId)
    {
        lock (_elements)
        {
            switch (ConfigUseIndexCache)
            {
                case IndexCache.Instance:
                {
                    if (formerId != null)
                    {
                        _instanceCache.Remove(formerId);
                    }

                    if (inMemoryObject.Id != null)
                    {
                        _instanceCache[inMemoryObject.Id] = inMemoryObject;
                    }

                    break;
                }
            }
        }
    }

    /// <summary>
    /// Finds the classmodel to that fitting workspace by looking into the the TypeIndexLogic.
    /// If type index logic is not initialized, null is returned 
    /// </summary>
    /// <param name="metaClassUri">The uri of the metaclass being requested.</param>
    /// <returns>The found class model or null in case it is not found or not initialized</returns>
    public ClassModel? FindClassModel(string metaClassUri)
    {
        return TypeIndex?.FindClassModel(metaClassUri);
    }
}

#pragma warning restore CS0162 // Unreachable code detected