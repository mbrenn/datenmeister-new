using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Provider.DotNet;

namespace DatenMeister.Runtime
{
    /// <summary>
    /// This class stores a set of helper methods to access, evaluate and work on
    /// real .Net objects
    /// </summary>
    public static class DotNetHelper
    {
        /// <summary>
        /// Evaluates whether the given type is a primitive type. 
        /// A primitive type is considered all numbers, strings, timespan and DateTime
        /// </summary>
        /// <param name="type">Type to be evaluated</param>
        /// <returns>true, if the given type is a primitive type</returns>
        public static bool IsPrimitiveType(Type type)
        {
            return type == typeof(bool)
                   || type == typeof(float)
                   || type == typeof(double)
                   || type == typeof(byte)
                   || type == typeof(short)
                   || type == typeof(int)
                   || type == typeof(long)
                   || type == typeof(string)
                   || type == typeof(TimeSpan)
                   || type == typeof(DateTime);
        }

        /// <summary>
        /// Evaluates whether the given type is a primitive type. 
        /// A primitive type is considered all numbers, strings, timespan and DateTime
        /// </summary>
        /// <param name="value">Value to be evaluated</param>
        /// <returns>true, if the given type is a primitive type</returns>
        public static bool IsOfPrimitiveType(object value)
        {
            return value != null && IsPrimitiveType(value.GetType());
        }

        /// <summary>
        /// Evaluates whether the given type is an enumeration but is not a string
        /// </summary>
        /// <param name="type">Type to be evaluated</param>
        /// <returns>true, if an enumeration and not a string</returns>
        public static bool IsEnumeration(Type type)
        {
            return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }
        
        /// <summary>
        /// Evaluates whether the given type is an enumeration but is not a string
        /// </summary>
        /// <param name="value">Value to be evaluated</param>
        /// <returns>true, if an enumeration and not a string</returns>
        public static bool IsOfEnumeration(object value)
        {
            return value != null && IsEnumeration(value.GetType());
        }

        /// <summary>
        /// Evaluates whether the given argument is null
        /// </summary>
        /// <param name="value">Value to be evaluated </param>
        /// <returns>true, if null</returns>
        public static bool IsNull(object value)
        {
            return value == null;
        }

        /// <summary>
        /// Verifies whether the given type is a enum (not a class, not a struct).
        /// </summary>
        /// <param name="type">Type to be verified</param>
        /// <returns>true, if the given type is an enum</returns>
        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        /// <summary>
        /// Verifies whether the given element is an enum
        /// </summary>
        /// <param name="value">Value to be verified</param>
        /// <returns>true, if enum</returns>
        public static bool IsOfEnum(object value)
        {
            return value != null && IsEnum(value.GetType());
        }

        internal static object ToString(object value)
        {
            if (value is double valueAsDouble)
            {
                return valueAsDouble.ToString(CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }

        /// <summary>
        /// Evaluates whether the given argument is a mof object
        /// </summary>
        /// <param name="propertyValue">Value to be checked</param>
        /// <returns>true, if the given element is of the type</returns>
        public static bool IsOfMofObject(object propertyValue)
        {
            return propertyValue is IObject;
        }

        public static bool IsOfMofElement(object propertyValue)
        {
            return propertyValue is IElement;
        }

        public static bool IsOfNumber(object property)
        {
            var type = property.GetType();
            return IsNumber(type);
        }

        private static bool IsNumber(Type type)
        {
            return type == typeof(short)
                   || type == typeof(int)
                   || type == typeof(long)
                   || type == typeof(float)
                   || type == typeof(double)
                   || type == typeof(decimal);
        }

        /// <summary>
        /// True, if the given element is a boolean
        /// </summary>
        /// <param name="property">Property to be evaluated</param>
        /// <returns>true, if this is a boolean</returns>
        public static bool IsOfBoolean(object property)
        {
            var type = property.GetType();
            return IsBoolean(type);
        }

        private static bool IsBoolean(Type type)
        {
            return type == typeof(bool);
        }

        /// <summary>
        /// Returns true, if the given element is of type IReflectiveCollection or if it can implement this interface
        /// </summary>
        /// <param name="value">Value to be verified</param>
        /// <returns>true, if that is the case</returns>
        public static bool IsOfReflectiveCollection(object value)
        {
            var type = value.GetType();
            return IsReflectiveCollection(type);
        }

        /// <summary>
        /// Returns true, if the given element is of type IReflectiveCollection or if it can implement this interface
        /// </summary>
        /// <param name="type">Type to be verified</param>
        /// <returns>true, if that is the case</returns>
        private static bool IsReflectiveCollection(Type type)
        {
            return typeof(IReflectiveCollection).IsAssignableFrom(type);
        }

        /// <summary>
        /// Determines whether the given element is of PRovider OBject
        /// </summary>
        /// <param name="element">Element to be verified</param>
        /// <returns>true, if the given element is of type IProviderObject</returns>
        public static bool IsOfProviderObject(object element)
        {
            return element is IProviderObject;
        }

        /// <summary>
        /// Converts the given element to a mof element
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="extent">The extent being used to create and resolve the element</param>
        /// <param name="typeLookup">The type lookup </param>
        /// <returns>The converted element</returns>
        public static IElement ConvertToMofElement(
            object value,
            IUriExtent extent,
            IDotNetTypeLookup typeLookup = null)
        {
            return ConvertToMofElement(
                value,
                new ExtentResolver((MofExtent) extent),
                new MofFactory(extent),
                typeLookup);
        }


        /// <summary>
        /// Converts the given element to a mof element
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="resolver">The resolver being used to figure out the </param>
        /// <param name="factory">Factory being used to create the mof element</param>
        /// <param name="typeLookup">The type lookup being used define the type</param>
        /// <returns>The converted element</returns>
        public static IElement ConvertToMofElement(
            object value,
            IUriResolver resolver,
            IFactory factory,
            IDotNetTypeLookup typeLookup = null)
        {
            if (value == null)
            {
                return null;
            }
            
            // Creates the mof element for type
            IElement valueType = null;

            var typeUri = typeLookup?.ToElement(value.GetType());

            if (!string.IsNullOrEmpty(typeUri))
            {
                valueType = resolver.Resolve(typeUri);
            }

            var instanceValue = factory.create(valueType);

            // Now sets the properties
            var typeOfValue = value.GetType();
            foreach (var property in typeOfValue.GetProperties())
            {
                var propertyValue = property.GetValue(value);

                instanceValue.set(property.Name,
                    ConvertPropertyValue(propertyValue, resolver, factory, typeLookup));
            }

            return instanceValue;
        }

        /// <summary>
        /// Converts the given value to a property that can be directly added the mof element
        /// </summary>
        /// <param name="value">Value to be used</param>
        /// <param name="resolver">Uriresolver being used to find references and/or meta classes</param>
        /// <param name="factory">Factory being used to create a new instance</param>
        /// <param name="typeLookup">Dotnet Typelookup to convert the dotnet type to the correct meta class</param>
        /// <returns>The converted object that can directly be set. </returns>
        private static object ConvertPropertyValue(object value, IUriResolver resolver, IFactory factory, IDotNetTypeLookup typeLookup)
        {
            if (IsOfPrimitiveType(value))
            {
                return value;
            }

            if (IsOfEnumeration(value))
            {
                var propertyValueAsList = (IEnumerable) value;
                var list = new List<object>();
                foreach (var listItem in propertyValueAsList)
                {
                    if (IsOfPrimitiveType(listItem))
                    {
                        list.Add(listItem);
                    }
                    else
                    {
                        list.Add(ConvertToMofElement(listItem, resolver, factory, typeLookup));
                    }
                }

                return list;
            }

            return ConvertToMofElement(value, resolver, factory, typeLookup);
        }
    }
}