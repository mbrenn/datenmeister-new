using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.ManualMapping
{
    public class TypeMapping
    {
        public IElement metaClass { get; set; }

        public Func<object, IElement> CreateNewObject { get; set; }

        public Action<IElement> InitializeNewObject { get; set; }

        public Func<object, string> GetId { get; private set; }

        private Dictionary<string, PropertyMapping> Properties { get; } 
            = new Dictionary<string, PropertyMapping>();

        public TypeMapping(Func<object, string> getIdFunc)
        {
            GetId = getIdFunc;
        }

        public PropertyMapping AddProperty<TInstanceValue, TReturnValue>(
            string property,
            Func<TInstanceValue, TReturnValue> getFunc,
            Action<TInstanceValue, TReturnValue> setFunc)
        {
            var propertyMapping = new PropertyMapping
            {
                GetValueFunc = value => getFunc((TInstanceValue) value),
                SetValueFunc = (value, propertyValue) => setFunc((TInstanceValue) value, (TReturnValue) propertyValue),
                DefaultValue = default(TInstanceValue)
            };

            Properties[property] = propertyMapping;
            return propertyMapping;
        }

        /// <summary>
        /// Returns the property mapping for a certain property by name
        /// </summary>
        /// <param name="property">Name of the property being queried</param>
        /// <returns>The found mapping</returns>
        public PropertyMapping FindProperty(string property)
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