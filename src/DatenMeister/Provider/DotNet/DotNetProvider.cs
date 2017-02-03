using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.DotNet
{
    /// <summary>
    /// Implements the provider for the DotNet objects
    /// </summary>
    public class DotNetProvider : IProvider
    {
        private readonly IDotNetTypeLookup _typeLookup;
        private readonly object _syncObject = new object();
        
        private readonly List<object> _elements;

        private Dictionary<string, object> _idStorage = new Dictionary<string, object>();

        /// <summary>
        /// Stores the object that stores the properties
        /// </summary>
        //private readonly InMemoryObject _innerObject = new InMemoryObject();

        /// <summary>
        /// Initializes a new instance of the DotNetExtent class
        /// </summary>
        /// <param name="contextUri">Uri of the context</param>
        /// <param name="typeLookup">Looked up type</param>
        public DotNetProvider(IDotNetTypeLookup typeLookup)
        {
            _typeLookup = typeLookup;
            _elements = new List<object>();

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
                var type = _typeLookup.ToType(metaClassUri);
                return CreateElementOfType(metaClassUri, type);
            }
        }

        private DotNetProviderObject CreateElementOfType(string metaClassUri, Type type)
        {
            var result = Activator.CreateInstance(type);
            var providerObject = new DotNetProviderObject(this, _typeLookup, result, metaClassUri);
            _idStorage[providerObject.Id] = providerObject.GetNativeValue();

            return providerObject;
        }

        /// <inheritdoc />
        public IProviderObject CreateElement(IElement metaClass)
        {
            if (metaClass == null) throw new ArgumentNullException(nameof(metaClass));

            lock (_syncObject)
            {
                return CreateElement(metaClass.GetUri());
            }
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject valueAsObject, int index = -1)
        {
            lock (_syncObject)
            {
                var providerObject = valueAsObject as DotNetProviderObject;
                if (providerObject == null) throw new ArgumentNullException(nameof(providerObject));
                _idStorage[valueAsObject.Id] = providerObject.GetNativeValue();

                if (index == -1)
                {
                    _elements.Add(providerObject.GetNativeValue());
                }
                else
                {
                    _elements.Insert(index, providerObject.GetNativeValue());
                }
            }
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            object toBeDelected;
            if (_idStorage.TryGetValue(id, out toBeDelected))
            {
                return _elements.Remove(toBeDelected);
            }

            return false;
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
            object result;
            if (_idStorage.TryGetValue(id, out result))
            {
                return new DotNetProviderObject(this, _typeLookup, result);
            }

            return null;
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            throw new NotImplementedException();
        }
    }
}