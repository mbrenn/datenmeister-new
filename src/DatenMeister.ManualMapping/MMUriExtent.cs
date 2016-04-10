using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.ManualMapping
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
        /// <typeparam name="T"></typeparam>
        /// <param name="metaClass"></param>
        /// <returns></returns>
        public TypeMapping AddMappingForType<T>(IElement metaClass)
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
                metaClass = metaClass,
                CreateNewObject = () => Activator.CreateInstance<T>()
            };

            TypeMappings[metaClass] = typeMapping;
            return typeMapping;
        }

        public bool useContainment()
        {
            return false;
        }

        public IReflectiveSequence elements()
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
    }
}
