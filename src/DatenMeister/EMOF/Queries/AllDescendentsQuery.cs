using System;
using System.Collections;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Queries
{
    /// <summary>
    ///     This query returns all objects which are descendents (and sub-descendents)
    ///     of an extent, an object or a reflecive collection
    /// </summary>
    public class AllDescendentsQuery
    {
        /// <summary>
        ///     Gets all descendents of an object, but does not
        ///     return this object itself
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <returns>An enumeration of all object and its descendents</returns>
        public static IEnumerable<IObject> getDescendents(IObject element)
        {
            var elementAsIObjectExt = element as IObjectAllProperties;
            if (elementAsIObjectExt == null)
            {
                throw new InvalidOperationException("element is not of type IObjectExt");
            }

            foreach (var property in elementAsIObjectExt.getPropertiesBeingSet())
            {
                var value = element.get(property);
                if (value is IObject)
                {
                    // Value is an object... perfect!
                    yield return value as IObject;
                }

                if (value is IEnumerable && value.GetType() != typeof (string))
                {
                    // Value is a real enumeration. Unfortunately strings are also
                    // enumeration, but we would like to skip them. Their content
                    // would be skipped either.
                    var valueAsEnumerable = value as IEnumerable;
                    foreach (var innerValue in getDescendents(valueAsEnumerable))
                    {
                        yield return innerValue;
                    }
                }
            }
        }

        public static IEnumerable<IObject> getDescendents(IEnumerable valueAsEnumerable)
        {
            foreach (var element in valueAsEnumerable)
            {
                if (element is IObject)
                {
                    var elementAsIObject = element as IObject;
                    yield return elementAsIObject;

                    foreach (var value in getDescendents(elementAsIObject))
                    {
                        yield return value;
                    }
                }
            }
        }

        public static IEnumerable<IObject> getDescendents(IExtent extent)
        {
            return getDescendents(extent.elements());
        }
    }
}