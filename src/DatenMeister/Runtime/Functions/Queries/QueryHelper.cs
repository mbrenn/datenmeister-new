using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    public static class QueryHelper
    {
        /// <summary>
        /// Gets the first child object of the element while queryProperty's value is the same as queryValue.
        /// The name of the element's property containing a reflective collection is specified in childrenProperty
        /// </summary>
        /// <param name="element">The element whose properties are query</param>
        /// <param name="childrenProperty">The name of the property whose reflective collection is queried. If this value is null, all properties are queried, which contain a ReflectiveCollection</param>
        /// <param name="queryProperty">The property's value that should match to query Value</param>
        /// <param name="queryValue">The query value that should match.</param>
        /// <returns>The found object</returns>
        public static IObject GetChildWithProperty(
            IObject element,
            string childrenProperty,
            string queryProperty,
            object queryValue)
        {
            if (childrenProperty != null)
            {
                var reflectiveCollection = element.get<IReflectiveCollection>(childrenProperty);
                return GetChildWithProperty(reflectiveCollection, queryProperty, queryValue);
            }
            else
            {
                var withProperties = element as IObjectAllProperties ??
                                     throw new InvalidOperationException(
                                         "Given element is not of IObjectAllProperties and no childrenProperty is set");

                // No go through all
                return withProperties.getPropertiesBeingSet().Select(property => GetChildWithProperty(element, property, queryProperty, queryValue)).FirstOrDefault(result => result != null);
            }
        }
        /// <summary>
        /// Gets the first child object of the element while queryProperty's value is the same as queryValue.
        /// The name of the element's property containing a reflective collection is specified in childrenProperty
        /// </summary>
        /// <param name="reflectiveCollection">The reflective collection being queried for the property</param>
        /// <param name="queryProperty">The property's value that should match to query Value</param>
        /// <param name="queryValue">The query value that should match.</param>
        /// <returns>The found object</returns>
        public static IObject GetChildWithProperty(IReflectiveCollection reflectiveCollection, string queryProperty,
            object queryValue)
        {
            return reflectiveCollection
                .OfType<IObject>()
                .FirstOrDefault(x => x.isSet(queryProperty) && queryValue.Equals(x.get(queryProperty)));
        }
    }
}