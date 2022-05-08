using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;

namespace DatenMeister.WebServer.Controller
{
    public class ItemsControllerInternal
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ItemsControllerInternal(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public IElement? GetItemInternal(string workspaceId, string extentUri, string itemId,
            out MofJsonConverter converter)
        {
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri) as IUriExtent;
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
                if (_workspaceLogic.Resolve(itemUri, ResolveType.Default, false) is not IObject foundElement)
                    throw new InvalidOperationException($"Element '{itemUri}' is not found");

                return foundElement;
            }
            else
            {
                var workspace = _workspaceLogic.GetWorkspace(workspaceId);
                if (workspace == null) throw new InvalidOperationException($"Workspace '{workspaceId}' is not found");

                if (workspace.Resolve(itemUri, ResolveType.NoMetaWorkspaces) is not IObject foundElement)
                    throw new InvalidOperationException($"Element '{itemUri}' is not found");

                return foundElement;
            }
        }
    }
}