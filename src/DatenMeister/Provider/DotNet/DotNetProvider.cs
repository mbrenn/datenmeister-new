using System;
using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Provider.DotNet
{
    /// <summary>
    /// Implements the provider for the DotNet objects
    /// </summary>
    public class DotNetProvider : IProvider
    {
        private readonly IDotNetTypeLookup _typeLookup;

        private readonly object _syncObject = new object();
        
        private readonly List<DotNetProviderObject> _elements = new List<DotNetProviderObject>();

        /// <summary>
        /// Stores the object that stores the properties
        /// </summary>
        //private readonly InMemoryObject _innerObject = new InMemoryObject();

        /// <summary>
        /// Initializes a new instance of the DotNetExtent class
        /// </summary>
        /// <param name="typeLookup">Looked up type</param>
        public DotNetProvider(IDotNetTypeLookup typeLookup)
        {
            _typeLookup = typeLookup;

            /*
            if (typeLookup == null) throw new ArgumentNullException(nameof(typeLookup));
            if (string.IsNullOrEmpty(contextUri))
                throw new ArgumentException("Value cannot be null or empty.", nameof(contextUri));

            _contextUri = contextUri;
            // _navigator = new ExtentUrlNavigator<DotNetElement>(this);

            // Creates the Reflective seqeunce
            var reflectiveSequence =
                typeLookup.CreateDotNetReflectiveSequence(new List<object>(), this);
            _elements = new ReflectiveSequenceForExtent(
                this, 
                reflectiveSequence);*/
        }

        /// <inheritdoc />
        public IProviderObject CreateElement(string metaClassUri)
        {
            lock (_syncObject)
            {
                if (string.IsNullOrEmpty(metaClassUri))
                {
                    throw new InvalidOperationException(".Net-Provider requires a meta class");
                }

                var type = _typeLookup.ToType(metaClassUri);
                if (type == null)
                {
                    throw new InvalidOperationException("No metaclass with uri '" + metaClassUri + "' is known");
                }

                return CreateElementOfType(metaClassUri, type);
            }
        }

        private DotNetProviderObject CreateElementOfType(string metaClassUri, Type type)
        {
            var result = Activator.CreateInstance(type);
            var providerObject = new DotNetProviderObject(this, _typeLookup, result, metaClassUri);

            return providerObject;
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject valueAsObject, int index = -1)
        {
            lock (_syncObject)
            {
                var providerObject = valueAsObject as DotNetProviderObject;
                if (providerObject == null) throw new ArgumentNullException(nameof(providerObject));

                if (index == -1)
                {
                    _elements.Add(providerObject);
                }
                else
                {
                    _elements.Insert(index, providerObject);
                }
            }
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            lock (_syncObject)
            {
                return _elements.RemoveAll(x => x.Id == id) > 0;
            }
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            lock (_syncObject)
            {
                _elements.Clear();
            }
        }

        /// <inheritdoc />
        public IProviderObject Get(string id)
        {
            lock (_syncObject)
            {
                return _elements.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            foreach (var element in _elements)
            {
                yield return element;
            }
        }
    }
}