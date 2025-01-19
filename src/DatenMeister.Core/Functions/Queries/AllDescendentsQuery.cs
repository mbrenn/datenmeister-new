using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.Functions.Queries
{
    [Flags]
    public enum DescendentMode
    {
        OnlyComposites = 0x01, 
        IncludingItself = 0x02
    }
    
    public static class DescendentModeExtensions
    {
        public static bool HasFlagFast(this DescendentMode value, DescendentMode flag)
        {
            return (value & flag) != 0;
        }
    }

    /// <summary>
    ///     This query returns all objects which are descendents (and sub-descendents)
    ///     of an extent, an object or a reflecive collection
    /// </summary>
    public class AllDescendentsQuery
    {
        private readonly HashSet<IObject> _alreadyVisited = new();

        private AllDescendentsQuery()
        {
        }

        /// <summary>
        ///     Gets all descendents of an object, but does not
        ///     return this object itself
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <returns>An enumeration of all object and its descendents</returns>
        public static IEnumerable<IObject> GetDescendents(
            IObject element,
            DescendentMode descendentMode = 0)
        {
            var inner = new AllDescendentsQuery();
            foreach (var found in inner.GetDescendentsInternal(element, null, descendentMode))
            {
                yield return found;
            }
        }

        /// <summary>
        ///     Gets all descendents of an object, but does not
        ///     return this object itself
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <param name="byFollowingProperties">The properties that shall be followed</param>
        /// <returns>An enumeration of all object and its descendents</returns>
        public static IEnumerable<IObject> GetDescendents(
            IObject element,
            IEnumerable<string>? byFollowingProperties = null,
            DescendentMode descendentMode = 0)
        {
            var inner = new AllDescendentsQuery();
            foreach (var found in inner.GetDescendentsInternal(element, byFollowingProperties?.ToList(), descendentMode))
            {
                yield return found;
            }
        }

        public static IEnumerable<IObject> GetDescendents(
            IExtent extent,
            IEnumerable<string>? byFollowingProperties = null,
            DescendentMode descendentMode = 0)
        {
            var inner = new AllDescendentsQuery();
            foreach(var found in inner.GetDescendentsInternal(extent.elements(),
             byFollowingProperties?.ToList(),
             null, 
             descendentMode | DescendentMode.IncludingItself))
            {
                yield return found;
            }
        }

        public static IEnumerable<IObject> GetDescendents(
            IEnumerable enumeration,
            IEnumerable<string>? byFollowingProperties = null,
            DescendentMode descendentMode = 0)
        {
            var inner = new AllDescendentsQuery();
            foreach (var found in inner.GetDescendentsInternal(enumeration, byFollowingProperties?.ToList(), null, descendentMode))
            {
                yield return found;
            }
        }

        public static IEnumerable<IObject> GetCompositeDescendents(
            IEnumerable enumeration,
            IEnumerable<string>? byFollowingProperties = null)
        {
            var inner = new AllDescendentsQuery();
            foreach (var found in inner.GetDescendentsInternal(enumeration, byFollowingProperties?.ToList(), null, DescendentMode.OnlyComposites))
            {
                yield return found;
            }
        }

        /// <summary>
        /// Gets the descendents of an element by following the properties and the element itself
        /// </summary>
        /// <param name="element">The element that shall be evaluated</param>
        /// <param name="byFollowingProperties">The properties that are requested</param>
        /// <param name="descendentMode">Descendent mode to be evaluated</param>
        /// <returns>An enumeration of all descendent elements</returns>
        private IEnumerable<IObject> GetDescendentsInternal(
            IObject element,
            ICollection<string>? byFollowingProperties,
            DescendentMode descendentMode = 0)
        {
            if (_alreadyVisited.Contains(element))
            {
                yield break;
            }

            if (descendentMode.HasFlagFast(DescendentMode.IncludingItself))
            {
                yield return element;
            }

            var asMofObject = (MofObject) element;

            _alreadyVisited.Add(element);

            // Now go through the list
            var elementAsIObjectExt = (IObjectAllProperties) element;
            if (elementAsIObjectExt == null)
            {
                throw new InvalidOperationException("element is not of type IObjectAllProperties");
            }

            // Gets the property list
            IEnumerable<string> propertyList;
            if (descendentMode.HasFlagFast(DescendentMode.OnlyComposites))
            {
                var metaClass = (element as IElement)?.getMetaClass();
                if (metaClass == null)
                {
                    propertyList = new[] {"id", "name"};
                }
                else
                {
                    propertyList =
                        ClassifierMethods.GetCompositingProperties(metaClass)
                            .Select(x => NamedElementMethods.GetName(x));
                }
            }
            else
            {
                propertyList = elementAsIObjectExt.getPropertiesBeingSet();
            }

            // Goes through the found properties
            foreach (var property in propertyList)
            {
                if (byFollowingProperties?.Contains(property) == false
                    || !asMofObject.ProviderObject.IsPropertySet(property))
                {
                    // Skip the properties that are not defined in the given collection
                    continue;
                }

                var value =
                    MofObject.ConvertToMofObject(
                        asMofObject,
                        property,
                        asMofObject.ProviderObject.GetProperty(
                            property),
                        noReferences: true);

                if (value is IObject valueAsObject)
                {
                    if (_alreadyVisited.Contains(valueAsObject))
                    {
                        // Skip what is already visited
                        yield break;
                    }

                    var parentAsMofObject = element as MofObject;
                    var childAsMofObject = valueAsObject as MofObject;
                    if (parentAsMofObject?.ReferencedExtent != childAsMofObject?.ReferencedExtent
                        && parentAsMofObject != null)
                    {
                        continue;
                    }

                    // Value is an object... perfect!
                    foreach (var innerValue in GetDescendentsInternal(valueAsObject, byFollowingProperties,
                                 descendentMode | DescendentMode.IncludingItself))
                    {
                        yield return innerValue;
                    }
                }
                else if (value != null && DotNetHelper.IsOfEnumeration(value))
                {
                    // Value is a real enumeration. 
                    var valueAsEnumerable = (IEnumerable) value;
                    foreach (var innerValue in GetDescendentsInternal(valueAsEnumerable, byFollowingProperties, element,
                                 descendentMode | DescendentMode.IncludingItself))
                    {
                        if (_alreadyVisited.Contains(innerValue))
                        {
                            // Skip what is already visited
                            continue;
                        }

                        yield return innerValue;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the enumeration and returns all properties by following the enumeration
        /// </summary>
        /// <param name="valueAsEnumerable">Value as enumerable which shall be parsed</param>
        /// <param name="byFollowingProperties">The properties that are requested</param>
        /// <param name="descendentMode">Mode of how to descend</param>
        /// <param name="parent">Parent to be evaluated</param>
        /// <returns>The descendents of each list item of the enumeration</returns>
        private IEnumerable<IObject> GetDescendentsInternal(
            IEnumerable? valueAsEnumerable,
            ICollection<string>? byFollowingProperties,
            IObject? parent,
            DescendentMode descendentMode = 0)
        {
            var parentAsMofObject = parent as MofObject;

            if (valueAsEnumerable == null)
            {
                yield break;
            }

            if (valueAsEnumerable is MofReflectiveSequence reflectiveSequence)
            {
                foreach (var element in reflectiveSequence.Enumerate(true))
                {
                    var childAsMofObject = element as MofObject;
                    if (parentAsMofObject?.ReferencedExtent != childAsMofObject?.ReferencedExtent
                        && parentAsMofObject != null)
                    {
                        continue;
                    }

                    if (element is IObject elementAsIObject)
                    {
                        foreach (var value in GetDescendentsInternal(elementAsIObject, byFollowingProperties,
                                     descendentMode))
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
                    var childAsMofObject = element as MofObject;
                    if (parentAsMofObject?.ReferencedExtent != childAsMofObject?.ReferencedExtent
                        && parentAsMofObject != null)
                    {
                        continue;
                    }

                    if (element is IObject elementAsIObject)
                    {
                        foreach (var value in GetDescendentsInternal(elementAsIObject, byFollowingProperties,
                                     descendentMode))
                        {
                            yield return value;
                        }
                    }
                }
            }
        }
    }
}