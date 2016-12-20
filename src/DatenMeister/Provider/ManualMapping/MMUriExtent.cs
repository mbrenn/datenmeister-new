using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;

namespace DatenMeister.Provider.ManualMapping
{
    public class MMUriExtent : IUriExtent
    {
        private readonly string _uri;

        public Dictionary<IElement, TypeMapping> TypeMappings { get; }
            = new Dictionary<IElement, TypeMapping>();

        /// <summary>
        /// Stores the object that stores the properties of the given extent
        /// </summary>
        private readonly InMemoryObject _innerObject = new InMemoryObject();

        public MMUriExtent(string uri)
        {
            _uri = uri;
        }

        /// <summary>
        /// Adds a mapping for a specific type. 
        /// The properties of the type are not explicitly addedl 
        /// </summary>
        /// <typeparam name="T">Type being added</typeparam>
        /// <typeparam name="TValue">Type of the value being added</typeparam>
        /// <param name="metaClass">Metaclass which is used to 
        /// talk about the class</param>
        /// <param name="getIdFunc">The query function to retrieve the id of an element</param>
        /// <param name="initializeFunc">Defines the function that shall be called the function is initialized</param>
        /// <returns>The created type mapping</returns>
        public TypeMapping AddMappingForType<T, TValue>(IElement metaClass, Func<T, string> getIdFunc, Action<T> initializeFunc = null) where T: MMElement<TValue>, new()
        {
            if (metaClass == null)
            {
                throw new ArgumentNullException(nameof(metaClass));
            }

            if (TypeMappings.ContainsKey(metaClass))
            {
                throw new InvalidOperationException("Metaclass is already set in typemapping");
            }

            var typeMapping = new TypeMapping(x=> getIdFunc((T) x))
            {
                metaClass = metaClass
            };

            typeMapping.CreateNewObject = (value) => new T
            {
                Extent = this,
                TypeMapping = typeMapping,
                Value = (TValue) value
            };

            if (initializeFunc != null)
            {
                typeMapping.InitializeNewObject = x => { initializeFunc((T) x); };
            }

            TypeMappings[metaClass] = typeMapping;
            return typeMapping;
        }

        public bool useContainment()
        {
            return false;
        }

        public virtual IReflectiveSequence elements()
        {
            throw new NotImplementedException();
        }

        public string contextURI()
        {
            return _uri;
        }

        public string uri(IElement element)
        {
            throw new NotImplementedException();
        }

        public IElement element(string uri)
        {
            throw new NotImplementedException();
        }

        private TypeMapping GetMapping(IElement metaClass)
        {
            TypeMapping result;
            TypeMappings.TryGetValue(metaClass, out result);

            return result;
        }

        public object ConvertToElement(IElement metaClass, object value)
        {
            var mapping = GetMapping(metaClass);
            var result = mapping?.CreateNewObject(value);
            mapping?.InitializeNewObject(result);
            return result;
        }

        /// <inheritdoc />
        public bool @equals(object other)
        {
            return Equals(other);
        }

        /// <inheritdoc />
        public object get(string property)
        {
            return _innerObject.get(property);
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            _innerObject.set(property, value);
        }

        /// <inheritdoc />
        public bool isSet(string property)
        {
            return _innerObject.isSet(property);
        }

        /// <inheritdoc />
        public void unset(string property)
        {
            _innerObject.unset(property);
        }
    }
}
