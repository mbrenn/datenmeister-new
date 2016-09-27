using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.ManualMapping
{
    public class MMUriExtent : IUriExtent
    {
        private readonly string _uri;

        public Dictionary<IElement, TypeMapping> TypeMappings { get; }
            = new Dictionary<IElement, TypeMapping>();

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
        /// <returns>The created type mapping</returns>
        public TypeMapping AddMappingForType<T, TValue>(IElement metaClass) where T: MMElement<TValue>, new()
        {
            if (metaClass == null)
            {
                throw new ArgumentNullException(nameof(metaClass));
            }

            if (TypeMappings.ContainsKey(metaClass))
            {
                throw new InvalidOperationException("Metaclass is already set in typemapping");
            }

            var typeMapping = new TypeMapping
            {
                metaClass = metaClass
            };
            typeMapping.CreateNewObject = (value) => new T
            {
                Extent = this,
                TypeMapping = typeMapping,
                Value = (TValue) value
            };

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
            return GetMapping(metaClass)?.CreateNewObject(value);
        }
    }
}
