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
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Json;
using DatenMeister.Provider.ExtentManagement;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class ElementsController : ControllerBase
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ElementsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        [HttpGet("api/elements/get_name/{workspace}/{extenturi}/{itemid}")]
        public ActionResult<object> GetName(string workspace, string extenturi, string itemid)
        {
            var foundItem = _workspaceLogic.FindObject(workspace, extenturi, itemid);
            if (foundItem == null)
            {
                return NotFound();
            }

            return new {name = NamedElementMethods.GetName(foundItem)};
        }

        [HttpGet("api/elements/get_name/{uri}/{workspace?}")]
        public ActionResult<object> GetName(string uri, string? workspace)
        {
            IElement? foundItem;
            if (string.IsNullOrEmpty(workspace))
            {
                foundItem = _workspaceLogic.FindItem(HttpUtility.UrlDecode(uri));
            }
            else
            {
                foundItem =
                    _workspaceLogic.GetWorkspace(workspace)?.Resolve(HttpUtility.UrlDecode(uri), ResolveType.Default)
                        as IElement;
            }

            if (foundItem == null)
            {
                return NotFound();
            }

            return new
            {
                name = NamedElementMethods.GetName(foundItem),
                extentUri = foundItem.GetUriExtentOf()?.contextURI() ?? string.Empty,
                workspace = foundItem.GetUriExtentOf()?.GetWorkspace()?.id ?? string.Empty,
                itemId = (foundItem as IHasId)?.Id ?? string.Empty
            };
        }

        [HttpGet("api/elements/get_composites/{workspaceId?}/{itemUrl?}")]
        public ActionResult<ItemWithNameAndId[]> GetComposites(string? workspaceId, string? itemUrl)
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

                if (foundExtent == null)
                {
                    return NotFound();
                }

                var foundExtents =
                    foundExtent.getOrDefault<IReflectiveCollection>(_DatenMeister._Management._Workspace.extents);

                return
                    foundExtents
                        .OfType<IObject>()
                        .Select(@t =>
                        {
                            var x = ItemWithNameAndId.Create(@t)!;
                            var realExtent = _workspaceLogic.FindExtent(
                                workspaceId,
                                t.get<string>(_DatenMeister._Management._Extent.uri));
                            x.extentUri = (realExtent as IUriExtent)?.contextURI() ?? string.Empty;
                            if (t.isSet(_UML._CommonStructure._NamedElement.name))
                            {
                                x.name = realExtent.get<string>(_UML._CommonStructure._NamedElement.name);
                            }

                            if (string.IsNullOrEmpty(x.name))
                            {
                                x.name = (realExtent as IUriExtent)?.contextURI() ?? "Unknown";
                            }

                            return x;
                        })
                        .ToArray();
            }

            var workspace = _workspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                return NotFound();
            }

            if (workspace.Resolve(itemUrl, ResolveType.NoMetaWorkspaces) is not IObject foundItem)
            {
                return NotFound();
            }

            var packagedItems = DefaultClassifierHints.GetPackagedElements(foundItem);
            return packagedItems
                .OfType<IObject>()
                .Select(x => ItemWithNameAndId.Create(x)!).ToArray();
        }
    }
}