﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

namespace DatenMeister.Core.Extensions
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
                if (!(value is ExpandoObject))
                {
                    throw new InvalidOperationException("The given value is not of type ExpandObject");
                }

                if (((IDictionary<string, object>)value).ContainsKey("__wrappedObject__"))
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
                value.__wrappedObject__ = null!;
            }
        }

        public static IObject? GetWrappedObject(dynamic value)
        {
            if (value == null)
            {
                return null;
            }

            return value.__wrappedObject__ as IObject;
        }

        /// <summary>
        /// Converts the given value to a dynamic object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="wrapInObject">Flag, indicating whether the value itself shall be wrapped in</param>
        /// <returns>Created object</returns>
        public static dynamic ToDynamic(IObject value, bool wrapInObject)
        {
            var result = new ExpandoObject();
            if (!(value is IObjectAllProperties allProperties))
            {
                throw new ArgumentException("value is not of type IObjectAllProperties.");
            }

            foreach (var property in allProperties.getPropertiesBeingSet())
            {
                var propertyValue = value.get(property);
                ((IDictionary<string, object?>)result)[property] = ConvertValue(propertyValue, wrapInObject);
            }

            if (wrapInObject)
            {
                SetWrappedObject(result, value);
            }

            return result;
        }

        private static object? ConvertValue(object? propertyValue, bool wrapInObject)
        {
            if (propertyValue == null) return null;
            if (DotNetHelper.IsOfPrimitiveType(propertyValue)) return propertyValue;
            if (DotNetHelper.IsOfMofObject(propertyValue)) return ToDynamic((propertyValue as IObject)!, wrapInObject);

            if (DotNetHelper.IsOfEnumeration(propertyValue))
            {
                var enumeration = propertyValue as IEnumerable;
                Debug.Assert(enumeration != null, "enumeration != null");

                var result = new List<object>();
                foreach (var innerValue in enumeration!)
                {
                    var convertedValue = ConvertValue(innerValue, wrapInObject);

                    if (convertedValue != null)
                    {
                        result.Add(convertedValue);
                    }
                }

                return result;
            }

            throw new InvalidOperationException($"Not handled due to unknown type: {propertyValue}");
        }
    }
}