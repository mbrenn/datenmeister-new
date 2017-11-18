using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    /// <summary>
    ///     This query returns all objects which are descendents (and sub-descendents)
    ///     of an extent, an object or a reflecive collection
    /// </summary>
    public class AllDescendentsQuery
    {
        private readonly HashSet<IObject> _alreadyVisited = new HashSet<IObject>();

        private AllDescendentsQuery()
        {
                
        }

        /// <summary>
        ///     Gets all descendents of an object, but does not
        ///     return this object itself
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <returns>An enumeration of all object and its descendents</returns>
        public static IEnumerable<IObject> GetDescendents(IObject element, IEnumerable<string> byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(element, byFollowingProperties.ToList());
        }

        public static IEnumerable<IObject> GetDescendents(IExtent extent, IEnumerable<string> byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(extent.elements(), byFollowingProperties.ToList());
        }

        public static IEnumerable<IObject> GetDescendents(IEnumerable enumeration, IEnumerable<string> byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(enumeration, byFollowingProperties.ToList());
        }

        private IEnumerable<IObject> GetDescendentsInternal(IObject element, ICollection<string> byFollowingProperties)
        {
            if (_alreadyVisited.Contains(element))
            {
                yield break;
            }

            yield return element;

            var asMofObject = (MofObject) element;

            _alreadyVisited.Add(element);

            // Now go through the list
            var elementAsIObjectExt = (IObjectAllProperties) element;
            if (elementAsIObjectExt == null)
            {
                throw new InvalidOperationException("element is not of type IObjectExt");
            }

            foreach (var property in elementAsIObjectExt.getPropertiesBeingSet())
            {
                if (byFollowingProperties != null && !byFollowingProperties.Contains(property))
                {
                    // Skip the properties that are not defined in the given collection
                    continue;
                }

                var value = asMofObject.get(property, noReferences: true);

                if (value is IObject)
                {
                    // Value is an object... perfect!
                    foreach (var innerValue in GetDescendentsInternal(value as IObject, byFollowingProperties))
                    {
                        yield return innerValue;
                    }
                }
                else if (DotNetHelper.IsOfEnumeration(value))
                {
                    // Value is a real enumeration. Unfortunately strings are also
                    // enumeration, but we would like to skip them. Their content
                    // would be skipped either.
                    var valueAsEnumerable = value as IEnumerable;
                    foreach (var innerValue in GetDescendentsInternal(valueAsEnumerable, byFollowingProperties))
                    {
                        yield return innerValue;
                    }
                }
            }
        }

        private IEnumerable<IObject> GetDescendentsInternal(IEnumerable valueAsEnumerable, ICollection<string> byFollowingProperties)
        {
            foreach (var element in valueAsEnumerable)
            {
                if (element is IObject)
                {
                    var elementAsIObject = element as IObject;
                    foreach (var value in GetDescendentsInternal(elementAsIObject, byFollowingProperties))
                    {
                        yield return value;
                    }
                }
            }
        }
    }
}