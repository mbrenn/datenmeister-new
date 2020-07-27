using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider;
using DatenMeister.Uml.Helper;

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
        /// <param name="byFollowingProperties">The properties that shall be followed</param>
        /// <returns>An enumeration of all object and its descendents</returns>
        public static IEnumerable<IObject> GetDescendents(IObject element, IEnumerable<string>? byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(element, byFollowingProperties?.ToList());
        }

        public static IEnumerable<IObject> GetDescendents(IExtent extent, IEnumerable<string>? byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(extent.elements(), byFollowingProperties?.ToList());
        }

        public static IEnumerable<IObject> GetDescendents(IEnumerable enumeration, IEnumerable<string>? byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(enumeration, byFollowingProperties?.ToList());
        }

        public static IEnumerable<IObject> GetCompositeDescendents(IEnumerable enumeration, IEnumerable<string>? byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            return inner.GetDescendentsInternal(enumeration, byFollowingProperties?.ToList(), true);
        }

        private IEnumerable<IObject> GetDescendentsInternal(IObject element, ICollection<string>? byFollowingProperties, bool onlyComposites = false)
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

            // Gets the property list
            IEnumerable<string> propertyList;
            if (onlyComposites)
            {
                var metaClass = (element as IElement)?.getMetaClass();
                if (metaClass == null)
                {
                    propertyList = new[] {"id", "name"};
                }
                else
                {
                    propertyList = ClassifierMethods.GetCompositingProperties(metaClass)
                        .Select(NamedElementMethods.GetName);
                }
            }
            else
            {
                propertyList = elementAsIObjectExt.getPropertiesBeingSet();
            }
            
            // Goes through the found properties
            foreach (var property in propertyList)
            {
                if (byFollowingProperties?.Contains(property) == false || !asMofObject.isSet(property))
                {
                    // Skip the properties that are not defined in the given collection
                    continue;
                }
                
                var value = asMofObject.get(property, noReferences: true, ObjectType.None);

                if (value is IObject valueAsObject)
                {
                    // Value is an object... perfect!
                    foreach (var innerValue in GetDescendentsInternal(valueAsObject, byFollowingProperties, onlyComposites))
                    {
                        yield return innerValue;
                    }
                }
                else if (value != null && DotNetHelper.IsOfEnumeration(value))
                {
                    // Value is a real enumeration. Unfortunately strings are also
                    // enumeration, but we would like to skip them. Their content
                    // would be skipped either.
                    var valueAsEnumerable = (IEnumerable) value;
                    foreach (var innerValue in GetDescendentsInternal(valueAsEnumerable, byFollowingProperties, onlyComposites))
                    {
                        yield return innerValue;
                    }
                }
            }
        }

        private IEnumerable<IObject> GetDescendentsInternal(IEnumerable valueAsEnumerable, ICollection<string>? byFollowingProperties, bool onlyComposites = false)
        {
            if (valueAsEnumerable == null)
            {
                yield break;
            }
            
            if (valueAsEnumerable is MofReflectiveSequence reflectiveSequence)
            {
                foreach (var element in reflectiveSequence.Enumerate(true))
                {
                    if (element is IObject elementAsIObject)
                    {
                        foreach (var value in GetDescendentsInternal(elementAsIObject, byFollowingProperties, onlyComposites))
                        {
                            yield return value;
                        }
                    }
                }
            }
            else
            {
                foreach (var element in valueAsEnumerable)
                {
                    if (element is IObject elementAsIObject)
                    {
                        foreach (var value in GetDescendentsInternal(elementAsIObject, byFollowingProperties, onlyComposites))
                        {
                            yield return value;
                        }
                    }
                }
            }
        }
    }
}