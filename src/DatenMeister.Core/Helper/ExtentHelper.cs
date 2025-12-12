using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Helper;

public static class ExtentHelper
{
    /// <summary>
    /// Defines the logger
    /// </summary>
    public static readonly ClassLogger Logger = new(typeof(ExtentHelper));

    public static ExtentConfiguration GetConfiguration(this IExtent extent)
        => (extent as IHasExtentConfiguration)?.ExtentConfiguration
           ?? throw new InvalidOperationException("Configuration is not existing");

    /// <summary>
    /// Returns the element, representing the .Net class
    /// </summary>
    /// <param name="extent">Extent to be used as base</param>
    /// <param name="type">Type to be resolves</param>
    /// <returns>The element being resolved</returns>
    public static IElement? ToResolvedElement(this MofExtent extent, Type type)
    {
        var element = extent.TypeLookup.ToElement(type);
        if (element == null || string.IsNullOrEmpty(element))
            return null;

        return extent.GetUriResolver().ResolveElement(element, ResolveType.Default);
    }

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
            return mofUriExtent.ResolveElement(shadow.Uri, ResolveType.Default);
        }

        Logger.Error(
            $"Given element is not of type MofElement or MofObjectShadow: {element.ToString() ?? "'null'"}");
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
        ArgumentNullException.ThrowIfNull(elements);

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
    /// Checks whether the requested id is still available in the extent.
    /// If that's the case, the element receives the given id, otherwise, the id will get a unique suffix
    /// </summary>
    /// <param name="extent">Extent to which the element is planned to be added</param>
    /// <param name="element">Element which is planned to be added</param>
    /// <param name="requestedId">Requested id</param>
    /// <returns>The actual id being added</returns>
    public static string SetAvailableId(IUriExtent extent, IElement element, string requestedId)
    {
        if (!(element is ICanSetId canSetId))
            throw new InvalidOperationException("element is not of type ICanSetId");

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

    /// <summary>
    /// Returns the item identified by path and workspace
    /// </summary>
    /// <param name="workspaceLogic">Workspace Logic to be used</param>
    /// <param name="workspaceId">Workspace to be used</param>
    /// <param name="path">Path to be used</param>
    /// <returns>Found Item or null</returns>
    public static IObject? TryGetItemByWorkspaceAndPath(
        IWorkspaceLogic workspaceLogic, string workspaceId, string path)
    {
        var workspace = workspaceLogic.GetWorkspace(workspaceId);
        var sourceElement = workspace?.Resolve(path, ResolveType.IncludeWorkspace);

        return sourceElement is not IObject asElement ? null : asElement;
    }
}