using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.TypeIndexAssembly;

#pragma warning disable CS0162 // Unreachable code detected (Compile-Time Configurations)

namespace DatenMeister.Core.Provider.InMemory;

/// <summary>
/// Stores all elements in the memory
/// </summary>
public class InMemoryProviderLinkedList : IProviderWithTypeIndex, ICanUpdateCacheId
{
    /// <summary>
    /// Stores the elements of the current provider
    /// </summary>
    private readonly LinkedList<InMemoryObject> _elements = [];
        
    /// <summary>
    /// Stores the instances for the objects within the cache
    /// </summary>
    private readonly Dictionary<string, LinkedListNode<InMemoryObject>> _instanceCache = new();
        
    /// <summary>
    ///     Stores the capabilities of the provider
    /// </summary>
    /// <returns></returns>
    private readonly ProviderCapability _providerCapability = new()
    {
        IsTemporaryStorage = true
    };

    /// <inheritdoc />
    public IProviderObject CreateElement(string? metaClassUri) =>
        new InMemoryObject(this, metaClassUri);
    

    public IProvider Provider => ProviderWithTypeIndex;
    
    public IProviderWithTypeIndex ProviderWithTypeIndex { get; set; }

    /// <summary>
    /// Stores the context to retrieve information about types from the typeindex logic.
    /// </summary>
    public TypeIndexInWorkspaceContext? TypeIndex { get; set; }

    /// <inheritdoc />
    public void AddElement(IProviderObject? valueAsObject, int index = -1)
    {
        if (valueAsObject == null)
            return; // Wo do not add empty elements

        lock (_elements)
        {
            LinkedListNode<InMemoryObject> newNode;
            var toBeAdded = (InMemoryObject)valueAsObject;
            if (index == -1)
            {
                newNode = _elements.AddLast(toBeAdded);
            }
            else
            {                    
                var nodeElement = _elements.First; // Start from the first node
                for (var i = 0; i < index; i++)
                {
                    // Move to the next node 'index' times
                    nodeElement = nodeElement?.Next; 
                }

                // After the loop, nodeElement should be the node at the desired index
                if (nodeElement == null)
                {
                    // This should theoretically not happen if index is within bounds and list is not empty,
                    // but good practice to check. Could occur if list modified concurrently without locks.
                    throw new InvalidOperationException("Could not find the node element at the specified index.");
                }

                if (nodeElement == null)
                {
                    throw new InvalidOperationException("Could not find the node element");
                }
                    
                // Ok, we found the right element, now add it
                newNode = _elements.AddAfter(nodeElement, toBeAdded);
            }

            // Updates the index
            var id = toBeAdded.Id;
            if (id != null)
            {
                _instanceCache[id] = newNode;
            }
        }
    }

    /// <inheritdoc />
    public bool DeleteElement(string id)
    {
        lock (_elements)
        {
            if (_instanceCache.Remove(id, out var element))
            {
                _elements.Remove(element);
                return true;
            }

            return false;
        }
    }

    /// <inheritdoc />
    public void DeleteAllElements()
    {
        lock (_elements)
        {
            _elements.Clear();
            _instanceCache.Clear();
        }
    }

    /// <inheritdoc />
    public IProviderObject? Get(string? id)
    {
        lock (_elements)
        {

            if (id == null)
            {
                return null;
            }

            return _instanceCache.GetValueOrDefault(id)?.Value;
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
            if (formerId != null)
            {
                _instanceCache.Remove(formerId);
            }

            if (inMemoryObject.Id != null)
            {
                var foundElement = _elements.Find(inMemoryObject);
                if (foundElement != null)
                {
                    _instanceCache[inMemoryObject.Id] = foundElement;    
                }
            }
        }
    }
}

#pragma warning restore CS0162 // Unreachable code detected