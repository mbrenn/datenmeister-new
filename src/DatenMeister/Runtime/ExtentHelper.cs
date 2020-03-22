#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime
{
    public static class ExtentHelper
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        public static readonly ClassLogger Logger = new ClassLogger(typeof(ExtentHelper));

        private const string DatenmeisterDefaultTypePackage = "__DatenMeister.DefaultTypePackage";

        private const string ExtentType = "__ExtentType";

        /// <summary>
        /// Sets the extent type
        /// </summary>
        /// <param name="extent">Extent, which shall get a type</param>
        /// <param name="extentType">Type of the extent to be set</param>
        public static void SetExtentType(this IExtent extent, string extentType)
        {
            extent.set(ExtentType, extentType);
        }

        /// <summary>
        /// Gets the extent type
        /// </summary>
        /// <param name="extent">Type of the extent to be set</param>
        public static string GetExtentType(this IExtent extent)
            => extent?.getOrDefault<string>(ExtentType) ?? string.Empty;

        /// <summary>
        /// Sets the default type package which is shown, when the user wants
        /// to create a new item
        /// </summary>
        /// <param name="extent">Extent shall get the default type package</param>
        /// <param name="defaultTypePackages">The elements which shall be considered as the
        /// default type package</param>
        public static void SetDefaultTypePackages(this IExtent extent, IEnumerable<IElement> defaultTypePackages)
        {
            extent.set(
                DatenmeisterDefaultTypePackage,
                defaultTypePackages);
        }

        public static void AddDefaultTypePackages(this IExtent extent, IEnumerable<IElement> defaultTypePackages)
        {
            var found = GetDefaultTypePackages(extent)?.ToList() ?? new List<IElement>();
            foreach (var newPackage in defaultTypePackages.Where(newPackage => !found.Contains(newPackage)))
            {
                found.Add(newPackage);
            }

            extent.SetDefaultTypePackages(found);
        }

        /// <summary>
        /// Gets the default type package
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        /// <returns>The found element</returns>
        public static IEnumerable<IElement> GetDefaultTypePackages(this IExtent extent) => 
            extent.get<IReflectiveCollection>(DatenmeisterDefaultTypePackage).OfType<IElement>();

        public static IElement? Resolve(this IExtent extent, IElement element)
        {
            if (!(extent is MofUriExtent mofUriExtent))
            {
                Logger.Error("Given extent is not of type MofUriExtent");
                return null;
            }

            if (element is MofElement)
            {
                return element;
            }

            if (element is MofObjectShadow shadow)
            {
                return mofUriExtent.Resolve(shadow.Uri, ResolveType.Default);
            }

            Logger.Error($"Given element is not of type MofElement or MofObjectShadow: {element?.ToString() ?? "'null'"}");
            return null;
        }

        /// <summary>
        /// Gets the factory of the given extent
        /// </summary>
        /// <returns>The created factory</returns>
        public static IFactory GetFactory(this IExtent extent) => new MofFactory(extent);

        /// <summary>
        /// Gets the workspace of the certain extent if extent implements IHasWorkspace and workspace is correctly set
        /// </summary>
        /// <param name="extent">Extent to be queried</param>
        /// <returns>The workspace correlated to the extent</returns>
        public static IWorkspace? GetWorkspace(this IExtent extent) => (extent as IHasWorkspace)?.Workspace;

        /// <summary>
        ///     Returns an enumeration of all columns that are within the given extent
        /// </summary>
        /// <param name="extent">Extent to be checked</param>
        /// <returns>Enumeration of all columns</returns>
        public static IEnumerable<string> GetProperties(this IUriExtent extent)
        {
            var elements = extent.elements();

            return GetProperties(elements);
        }

        /// <summary>
        /// Removes all elements within the reflective collection
        /// </summary>
        /// <param name="elements">Elements in which all elements shall be removed</param>
        public static void RemoveAll(this IReflectiveCollection elements)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            foreach (var element in elements.ToList())
            {
                elements.remove(element);
            }
        }

        /// <summary>
        /// Gets the unified amount of properties of all the elements within the given reflective collection
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetProperties(IReflectiveCollection elements)
        {
            var result = new List<object>();
            foreach (var item in elements)
            {
                if (item is IObjectAllProperties itemAsObjectExt)
                {
                    var properties = itemAsObjectExt.getPropertiesBeingSet();

                    foreach (var property in properties
                        .Where(property => !result.Contains(property)))
                    {
                        result.Add(property);
                        yield return property;
                    }
                }
            }
        }

        /// <summary>
        /// Finds out of an enumeration of extents, the extent that has the given element.
        /// </summary>
        /// <param name="extents">Extents to be parsed</param>
        /// <param name="value">Element to be found</param>
        /// <returns>the extent with the element or none</returns>
        public static IUriExtent? WithElement(this IEnumerable<IUriExtent> extents, IObject value)
        {
            // If the object is contained by another object, query the contained objects
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return WithElement(extents, parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            if (value is IHasExtent objectKnowsExtent)
            {
                var foundExtent = objectKnowsExtent.Extent as IUriExtent;
                return extents.FirstOrDefault(x => x == foundExtent);
            }

            // First, try to find it via the caching
            var uriExtents = extents as IList<IUriExtent> ?? extents.ToList();

            foreach (var extent in uriExtents)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (extent is IExtentCachesObject extentAsObjectCache)
                {
                    if (extentAsObjectCache.HasObject(value))
                    {
                        return extent;
                    }
                }
            }

            // If not successful, try to find it by traditional, but old approach
            foreach (var extent in uriExtents)
            {
                if (AllDescendentsQuery.GetDescendents(extent.elements())
                    .Any(x => x.Equals(value)))
                {
                    return extent;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds in the meta extents and meta workspaces the metaclass
        /// </summary>
        /// <typeparam name="TFilledType">Filled Type to be evaluated</typeparam>
        /// <param name="extent">Extent to be evaluated</param>
        /// <param name="type">Type converter finding the requested type</param>
        /// <returns>The found element</returns>
        public static IElement? FindInMeta<TFilledType>(this IExtent extent, Func<TFilledType, IElement> type)
            where TFilledType : class, new()
        {
            var filledType = ((MofExtent) extent).Workspace?.GetFromMetaWorkspace<TFilledType>();
            return filledType == null ? null : type(filledType);
        }

        /// <summary>
        /// Checks whether the requested id is still available in the extent.
        /// If that's the case, the element receives the given id, otherwise, the id will get a unique suffix
        /// </summary>
        /// <param name="extent">Extent to which the element is planned to be added</param>
        /// <param name="element">Element which is planned to be added</param>
        /// <param name="requestedId">Requested id</param>
        /// <returns>The actual id being added</returns>
        public static string SetAvailableId(IUriExtent extent, IElement element, string requestedId)
        {
            if (!(element is ICanSetId canSetId)) throw new InvalidOperationException("element is not of type ICanSetId");

            var testedId = requestedId;
            var currentNr = 0;

            do
            {
                var found = extent.element("#" + testedId);
                if (found == null)
                {
                    canSetId.Id = testedId;
                    return testedId;
                }

                currentNr++;
                testedId = requestedId + "-" + currentNr;

            } while (true);
        }
    }
}