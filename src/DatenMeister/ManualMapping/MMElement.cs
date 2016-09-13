using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.ManualMapping
{
    public class MMElement<T> : IElement, IHasValue
    {

        /// <summary>
        /// Gets or sets the extent being used to find subtypes
        /// </summary>
        public MMUriExtent Extent { get; internal set; }

        /// <summary>
        /// Stores the mapping
        /// </summary>
        public TypeMapping TypeMapping { get; internal set; }

        public T Value { get; internal set; }


        public IElement Container { get; }

        /// <summary>
        /// Initializes a new instance of the MMElement class. 
        /// This method shall only be used by internal classes. 
        /// </summary>
        public MMElement()
        {
            
        }

        public MMElement(TypeMapping typeMapping, T value, IElement container = null)
        {
            if (typeMapping == null) throw new ArgumentNullException(nameof(typeMapping));
            TypeMapping = typeMapping;
            Value = value;
            Container = container;
        }

        public bool equals(object other)
        {
            var otherAsMMElement = other as IHasValue;
            if (otherAsMMElement != null)
            {
                return Value.Equals(otherAsMMElement.ValueAsObject);
            }

            return Value.Equals(other);
        }

        public object get(string property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var mapping =  FindProperty(property);
            return mapping.GetValueFunc(Value);
        }

        public void set(string property, object value)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var mapping = FindProperty(property);
            mapping.SetValueFunc(Value, value);
        }

        public bool isSet(string property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            var mapping = TypeMapping.FindProperty(property);

            return mapping != null;
        }

        public void unset(string property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            throw new NotImplementedException();
        }

        public IElement metaclass => TypeMapping.metaClass;

        object IHasValue.ValueAsObject => Value;

        public IElement getMetaClass()
        {
            return TypeMapping.metaClass;
        }

        public IElement container()
        {
            return Container;
        }

        private PropertyMapping FindProperty(string property)
        {
            var mapping = TypeMapping.FindProperty(property);
            if (mapping == null)
            {
                throw new InvalidOperationException($"Mapping for '{property}' unknown.");
            }

            return mapping;
        }
    }
}