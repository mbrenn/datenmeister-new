using System;
using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Helper
{
    /// <summary>
    ///     This query returns all objects which are descendents (and sub-descendents)
    ///     of an extent, an object or a reflecive collection
    /// </summary>
    public class AllDescendentsQuery
    {
        private HashSet<IObject> _alreadyVisited = new HashSet<IObject>();

        private AllDescendentsQuery()
        {
                
        }

        /// <summary>
        ///     Gets all descendents of an object, but does not
        ///     return this object itself
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <returns>An enumeration of all object and its descendents</returns>
        public static IEnumerable<IObject> GetDescendents(IObject element)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(element);
        }

        public static IEnumerable<IObject> GetDescendents(IExtent extent)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(extent.elements());
        }

        public IEnumerable<IObject> GetDescendentsInternal(IObject element)
        {
            if (_alreadyVisited.Contains(element))
            {
                yield break;
            }

            _alreadyVisited.Add(element);


            // Now go through the list
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
                    foreach (var innerValue in GetDescendentsInternal(valueAsEnumerable))
                    {
                        yield return innerValue;
                    }
                }
            }
        }

        public static IEnumerable<IObject> GetDescendents(IEnumerable enumeration)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(enumeration);
        } 

        public IEnumerable<IObject> GetDescendentsInternal(IEnumerable valueAsEnumerable)
        {
            foreach (var element in valueAsEnumerable)
            {
                if (element is IObject)
                {
                    var elementAsIObject = element as IObject;
                    yield return elementAsIObject;

                    foreach (var value in GetDescendentsInternal(elementAsIObject))
                    {
                        yield return value;
                    }
                }
            }
        }
    }
}