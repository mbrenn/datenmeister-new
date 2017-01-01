using System;
using System.Collections.Generic;

namespace DatenMeister.Provider.InMemory
{
    public class InMemoryProvider : IProvider
    {
       // private List<InMemoryObject> _elements = new List<InMemoryObject>();

        /// <inheritdoc />
        public IProviderObject CreateElement(string metaClassUri)
        {
            var element = new InMemoryObject();
            return element;
        }

        public IProviderObject Get(string id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void DeleteElement(string id)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool IsPropertySet(string id, string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public object GetProperty(string id, string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<string> GetProperties(string id)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool DeleteProperty(string id, string property)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void SetProperty(string id, string property, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool AddToProperty(string id, string property, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public bool RemoveFromProperty(string id, string property, object value)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<ElementReference> GetRootObjects()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<ElementReference> GetAllObjects()
        {
            throw new System.NotImplementedException();
        }
    }
}