using System;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.ManualMapping
{
    public class MMElement : IElement
    {
        /// <summary>
        /// Stores the mapping
        /// </summary>
        private TypeMapping _typeMapping;

        private readonly object _instance;

        private readonly IElement _container;

        public MMElement(TypeMapping typeMapping, object value, IElement container = null)
        {
            if (typeMapping == null) throw new ArgumentNullException(nameof(typeMapping));
            _typeMapping = typeMapping;
            _instance = value;
            _container = container;
        }

        public bool @equals(object other)
        {
            var otherAsMMElement = other as MMElement;
            if (otherAsMMElement != null)
            {
                return _instance.Equals(otherAsMMElement._instance);
            }

            return _instance.Equals(other);
        }

        public object get(object property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var mapping =  FindProperty(property);
            return mapping.GetValueFunc(_instance);
        }

        public void set(object property, object value)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var mapping = FindProperty(property);
            mapping.SetValueFunc(_instance, value);
        }

        public bool isSet(object property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            var mapping = _typeMapping.FindProperty(property);

            return mapping != null;
        }

        public void unset(object property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            throw new NotImplementedException();
        }

        public IElement metaclass => _typeMapping.metaClass;

        public IElement getMetaClass()
        {
            return _typeMapping.metaClass;
        }

        public IElement container()
        {
            return _container;
        }

        private PropertyMapping FindProperty(object property)
        {
            var mapping = _typeMapping.FindProperty(property);
            if (mapping == null)
            {
                throw new InvalidOperationException($"Mapping for '{property}' unknown.");
            }

            return mapping;
        }
    }
}