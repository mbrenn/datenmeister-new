using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Dynamic
{
    /// <summary>
    /// Converts the MOF instance to a dynamic object
    /// </summary>
    public static class DynamicConverter
    {
        /// <summary>
        /// Sets the wrapped object into the dynamic value. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="objectToBeSet"></param>
        public static void SetWrappedObject(dynamic value, IObject objectToBeSet)
        {
            if (value != null)
            {
                var valueAsExpandoObject = value as ExpandoObject;
                if (valueAsExpandoObject == null)
                {
                    throw new InvalidOperationException("The given value is not of type ExpandObject");
                }

                if (((IDictionary<string, object>) value).ContainsKey("__wrappedObject__"))
                {
                    throw new InvalidOperationException("Somehow, the object already has a value.");
                }

                value.__wrappedObject__ = objectToBeSet;
            }
        }

        public static void UnsetWrappedObject(dynamic value)
        {
            if (value != null)
            {
                value.__wrappedObject__ = null;
            }
        }

        public static IObject GetWrappedObject(dynamic value)
        {
            if (value == null)
            {
                return null;
            }

            return value.__wrappedObject__ as IObject;
        }

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

            SetWrappedObject(result, value);

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