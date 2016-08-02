using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.ManualMapping
{
    public class TypeMapping
    {
        public IElement metaClass { get; set; }

        public Func<object> CreateNewObject { get; set; }

        public Func<object, string> GetId { get; set; }

        public Dictionary<object, PropertyMapping> Properties { get; } 
            = new Dictionary<object, PropertyMapping>();

        public PropertyMapping AddProperty<T>(
            object property,
            Func<object, T> getFunc,
            Action<object, T> setFunc)
        {
            var propertyMapping = new PropertyMapping
            {
                GetValueFunc = value => getFunc(value),
                SetValueFunc = (value, propertyValue) => setFunc(value, (T) propertyValue),
                DefaultValue = default(T)
            };

            Properties[property] = propertyMapping;
            return propertyMapping;
        }

        public PropertyMapping FindProperty(object property)
        {
            PropertyMapping mapping;
            if (Properties.TryGetValue(property, out mapping))
            {
                return mapping;
            }

            return null;
        }
    }
}