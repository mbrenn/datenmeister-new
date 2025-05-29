using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView;
using DatenMeister.Web.Json;

namespace DatenMeister.WebServer.Controller;

public class ItemsControllerInternal(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    private IWorkspaceLogic WorkspaceLogic { get; } = workspaceLogic;

    public IElement GetItemInternal(string workspaceId, string extentUri, string itemId,
        out MofJsonConverter converter)
    {
        var extent = WorkspaceLogic.FindExtent(workspaceId, extentUri) as IUriExtent;
        if (extent == null) throw new InvalidOperationException("Extent is not found");

        var foundElement = extent.element("#" + itemId);
        if (foundElement == null) throw new InvalidOperationException("Element is not found");

        converter = new MofJsonConverter { MaxRecursionDepth = 2 };
        return foundElement;
    }

    /// <summary>
    ///     Gets the internal property of a certain item
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="itemUri">Uri of the item being handled</param>
    /// <param name="property">The property that shall be loaded</param>
    /// <returns>The found property</returns>
    public object? GetPropertyInternal(string workspaceId, string itemUri, string property)
    {
        var foundItem = GetItemByUriParameter(workspaceId, itemUri)
                        ?? throw new InvalidOperationException("Item was not found");

        object? result = null;
        if (foundItem.isSet(property)) result = foundItem.get(property);

        return result;
    }

    /// <summary>
    ///     Gets the items by the uri parameter.
    ///     The parameter themselves are expected to be uri-encoded, so a decoding via HttpUtility.UrlDecode will be performed
    /// </summary>
    /// <param name="workspaceId">Id of the workspace. Null, if through all workspaces shall be searched</param>
    /// <param name="itemUri">Uri of the item</param>
    /// <returns>The found object</returns>
    public IObject GetItemByUriParameter(string? workspaceId, string itemUri)
    {
        if (string.IsNullOrEmpty(workspaceId) || workspaceId == "_")
        {
            if (WorkspaceLogic.Resolve(itemUri, ResolveType.Default, false) is not IObject foundElement)
                throw new InvalidOperationException($"Element '{itemUri}' w/o Workspace is not found");

            return foundElement;
        }
        else
        {
            var workspace = WorkspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null) throw new InvalidOperationException($"Workspace '{workspaceId}' is not found");

            if (workspace.Resolve(itemUri, ResolveType.NoMetaWorkspaces) is not IObject foundElement)
                throw new InvalidOperationException($"Element '{itemUri}' in Workspace {workspaceId} is not found");

            return foundElement;
        }
    }

    public List<IObject>? GetRootElementsInternal(string workspaceId, string extentUri, string? viewNode = null, QueryFilterParameter? filterParameter = null)
    {
        var (collection, extent) = WorkspaceLogic.FindExtentAndCollection(workspaceId, extentUri);
        if (collection == null || extent == null)
        {
            return null;
        }

        /*
         * Checks, if a view node was specified, if a view node was specified, the elements will be filtered
         * according the viewnode
         */
        if (viewNode != null)
        {
            var dataviewHandler =
                new DataViewEvaluation(WorkspaceLogic, scopeStorage);
            dataviewHandler.AddDynamicSource("input", collection);

            var viewNodeElement =
                WorkspaceLogic.FindElement(WorkspaceNames.WorkspaceManagement, viewNode)
                ?? WorkspaceLogic.FindElement(WorkspaceNames.WorkspaceData, viewNode);

            if (viewNodeElement != null)
            {
                collection = dataviewHandler.GetElementsForViewNode(viewNodeElement);
            }
            else
            {
                return null;
            }
        }

        var finalElements = collection.OfType<IObject>().ToList();

        // Evaluates the filter parameters
        if (filterParameter != null && !string.IsNullOrEmpty(filterParameter.OrderBy))
        {
            var sorter = new PropertyComparer(filterParameter.OrderBy, filterParameter.OrderByDescending);

            finalElements = finalElements.Order(sorter).ToList();
        }

        // Evaluate the filters from the query itself by going through all the filter properties
        if (filterParameter?.FilterByProperties != null && filterParameter.FilterByProperties.Count() > 0)
        {
            foreach (var filter in filterParameter.FilterByProperties)
            {
                finalElements = finalElements.Where(x => x.isSet(filter.Key) && x.get(filter.Key)?.ToString() == filter.Value).ToList();
            }
        }

#if DEBUG
#warning Number of elements in ItemsController is limited to improve speed during development. This is not a release option
        return finalElements.Take(100).ToList();
#else
            return finalElements;
#endif

    }

    private class PropertyComparer(string property, bool descending) : IComparer<IObject>
    {
        public int Compare(IObject? x, IObject? y)
        {
            var isPropertyX = x == null || !x.isSet(property);
            var isPropertyY = y == null || !y.isSet(property);
            var factor = descending ? -1 : 1;

            // Checks whether the values are existing
            // Per definition, these values will be sorted to the end of the table
            if (isPropertyX && isPropertyY)
            {
                return 0;
            }

            if (isPropertyY)
            {
                return -1;
            }

            if (isPropertyX)
            {
                return 1;
            }

            // Checks the content of these values
            var propertyX = x!.get(property);
            var propertyY = y!.get(property);

            // Null properties will go to the bottom
            if (propertyX == null && propertyY == null)
            {
                return 0;
            }

            if (propertyY == null)
            {
                return -1;
            }

            if (propertyX == null)
            {
                return 1;
            }

            // Checks the content
            if (propertyX is int intX && propertyY is int intY)
            {
                return factor * intX.CompareTo(intY);
            }

            if (propertyX is double doubleX && propertyY is double doubleY)
            {
                return factor * doubleX.CompareTo(doubleY);
            }

            return factor * propertyX.ToString()!.CompareTo(propertyY.ToString());
        }
    }
    public static Dictionary<string, string> DeserializeStringToDictionary(string serializedString)
    {
        var decodedString = Uri.UnescapeDataString(serializedString);
        var result = new Dictionary<string, string>();
        var pairs = serializedString.Split('&');
        foreach (var pair in pairs)
        {
            var keyValue = pair.Split('=');
            if (keyValue.Length == 2)
            {
                var key = Uri.UnescapeDataString(keyValue[0]);
                var value = Uri.UnescapeDataString(keyValue[1]);
                result[key] = value;
            }
        }
        return result;
    }
}