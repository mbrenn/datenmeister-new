using System;
using System.Linq;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;
using DatenMeister.Provider.ExtentManagement;

namespace DatenMeister.WebServer.Controller
{
    public class ElementsControllerInternal
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ElementsControllerInternal(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }


        public ItemWithNameAndId[]? GetComposites(string? workspaceId, string? itemUrl)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUrl = HttpUtility.UrlDecode(itemUrl);

            if (workspaceId == null)
            {
                var extent = ManagementProviderPlugin.GetExtentForWorkspaces(_workspaceLogic);
                var rootElements = extent.elements();

                return rootElements.OfType<IObject>().Select(x => ItemWithNameAndId.Create(x)!).ToArray();
            }

            if (itemUrl == null)
            {
                var workspaceExtent = ManagementProviderPlugin.GetExtentForWorkspaces(_workspaceLogic);
                var rootElements = workspaceExtent.elements();

                var foundExtent =
                    rootElements
                        .OfType<IObject>()
                        .FirstOrDefault(x =>
                            x.getOrDefault<string>(_DatenMeister._Management._Workspace.id) == workspaceId);

                if (foundExtent == null) return null;

                var foundExtents =
                    foundExtent.getOrDefault<IReflectiveCollection>(_DatenMeister._Management._Workspace.extents);

                return
                    foundExtents
                        .OfType<IObject>()
                        .Select(t =>
                        {
                            var x = ItemWithNameAndId.Create(t)!;
                            var realExtent = _workspaceLogic.FindExtent(
                                workspaceId,
                                t.get<string>(_DatenMeister._Management._Extent.uri));
                            x.extentUri = (realExtent as IUriExtent)?.contextURI() ?? string.Empty;
                            if (t.isSet(_UML._CommonStructure._NamedElement.name))
                                x.name = realExtent.get<string>(_UML._CommonStructure._NamedElement.name);

                            if (string.IsNullOrEmpty(x.name))
                                x.name = (realExtent as IUriExtent)?.contextURI() ?? "Unknown";

                            return x;
                        })
                        .ToArray();
            }

            var workspace = _workspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null) return null;

            if (workspace.Resolve(itemUrl, ResolveType.NoMetaWorkspaces) is not IObject foundItem) return null;

            var packagedItems = DefaultClassifierHints.GetPackagedElements(foundItem);
            return packagedItems
                .OfType<IObject>()
                .Select(x => ItemWithNameAndId.Create(x)!).ToArray();
        }

        /// <summary>
        ///     Performs a search by the search string
        /// </summary>
        /// <param name="search">The string being typed in by the user</param>
        /// <returns>The answer by the logic. </returns>
        public ElementsController.OutFindBySearchString FindBySearchString(string search)
        {
            var result = new ElementsController.OutFindBySearchString();
            var found = _workspaceLogic.Resolve(search, ResolveType.Default, false);
            if (found is IUriExtent asExtent)
            {
                result.resultType = ElementsController.OutFindBySearchString.ResultTypeReferenceExtent;
                result.reference = ItemWithNameAndId.Create(asExtent)
                                   ?? throw new InvalidOperationException("Create could not handle an object...");
            }
            else if (found is IObject asObject)
            {
                result.resultType = ElementsController.OutFindBySearchString.ResultTypeReference;
                result.reference = ItemWithNameAndId.Create(asObject)
                                   ?? throw new InvalidOperationException("Create could not handle an object...");
            }
            else
            {
                result.resultType = ElementsController.OutFindBySearchString.ResultTypeNone;
            }

            return result;
        }
    }
}