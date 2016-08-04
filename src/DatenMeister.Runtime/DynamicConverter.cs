using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime
{
    /// <summary>
    /// Converts the MOF instance to a dynamic object
    /// </summary>
    public static class DynamicConverter
    {
        public static dynamic ToDynamic(IObject value)
        {
            var result = new ExpandoObject();
            var allProperties = value as IObjectAllProperties;
            if (allProperties == null)
            {
                throw new ArgumentException("value is not of type IObjectAllProperties.");
            }
            
            foreach (var property in allProperties.getPropertiesBeingSet())
            {
                var propertyValue = value.get(property);
                ((IDictionary<string, object>) result)[property] = ConvertValue(propertyValue);
            }

            return result;
        }

        private static object ConvertValue(object propertyValue)
        {
            if (DotNetHelper.IsNull(propertyValue))
            {
                return null;
            }
            if (DotNetHelper.IsOfPrimitiveType(propertyValue))
            {
                return propertyValue;
            }
            if (DotNetHelper.IsOfMofObject(propertyValue))
            {
                return ToDynamic(propertyValue as IObject);
            }
            if (DotNetHelper.IsOfEnumeration(propertyValue))
            {
                var enumeration = propertyValue as IEnumerable;
                Debug.Assert(enumeration != null, "enumeration != null");

                var result = new List<object>();
                foreach (var innerValue in enumeration)
                {
                    result.Add(ConvertValue(innerValue));
                }

                return result;
            }

            throw new InvalidOperationException($"Not handled due to unknown type: {propertyValue}");
        }
    }
}