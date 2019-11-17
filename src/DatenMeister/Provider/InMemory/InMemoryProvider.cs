#nullable enable
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;

namespace DatenMeister.Provider.InMemory
{
    /// <summary>
    /// Stores all elements in the memory
    /// </summary>
    public class InMemoryProvider : IProvider
    {
        /// <summary>
        /// Stores the temporary extent that can be used to create temporary objects
        /// </summary>
        public static readonly MofUriExtent TemporaryExtent = new MofUriExtent(new InMemoryProvider(), "datenmeister:///temp");

        /// <summary>
        /// Gets the used temporary provider
        /// </summary>
        public static IProvider TemporaryProvider => (InMemoryProvider) TemporaryExtent.Provider;

        /// <summary>
        /// Stores the elements of the current provider
        /// </summary>
        private readonly List<InMemoryObject> _elements = new List<InMemoryObject>();

        /// <summary>
        /// Stores the memory object for lacal information
        /// </summary>
        private readonly InMemoryObject _extentElement;

        public InMemoryProvider()
        {
            _extentElement = new InMemoryObject(this);
        }

        /// <inheritdoc />
        public IProviderObject CreateElement(string metaClassUri) =>
            new InMemoryObject(this, metaClassUri);

        /// <inheritdoc />
        public void AddElement(IProviderObject valueAsObject, int index = -1)
        {
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
            }
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            lock (_elements)
            {
                return _elements.RemoveAll(x => x.Id == id) > 0;
            }
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            lock (_elements)
            {
                _elements.Clear();
            }
        }

        /// <inheritdoc />
        public IProviderObject Get(string id)
        {
            lock (_elements)
            {
                if (id == null)
                {
                    return _extentElement;
                }

                return _elements.First(x => x.Id == id);
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
        public ProviderCapability GetCapabilities() => ProviderCapability.None;
    }
}