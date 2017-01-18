using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;

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
            var type = _typeLookup.ToType(metaClassUri);
            var result = Activator.CreateInstance(type);
            return new DotNetProviderObject(this, _typeLookup, result, metaClassUri);
        }

        /// <inheritdoc />
        public void AddElement(IProviderObject valueAsObject, int index = -1)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool DeleteElement(string id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DeleteAllElements()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IProviderObject Get(string id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<IProviderObject> GetRootObjects()
        {
            throw new NotImplementedException();
        }
    }
}