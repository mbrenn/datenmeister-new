using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class ElementsController : ControllerBase
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ElementsControllerInternal _internal;

        public ElementsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            _internal = new ElementsControllerInternal(workspaceLogic, scopeStorage);
        }

        [HttpGet("api/elements/get_name/{workspace}/{extenturi}/{itemid}")]
        public ActionResult<object> GetName(string workspace, string extenturi, string itemid)
        {
            var foundItem = _workspaceLogic.FindObject(workspace, extenturi, itemid);
            if (foundItem == null)
            {
                return NotFound();
            }

            return new { name = NamedElementMethods.GetName(foundItem) };
        }

        [HttpGet("api/elements/get_name/{workspace}/{uri}")]
        public ActionResult<object> GetName(string? workspace, string uri)
        {
            IElement? foundItem;
            if (string.IsNullOrEmpty(workspace) || workspace == "_")
            {
                foundItem = _workspaceLogic.FindItem(HttpUtility.UrlDecode(uri));
            }
            else
            {
                foundItem =
                    _workspaceLogic.GetWorkspace(workspace)?.Resolve(HttpUtility.UrlDecode(uri), ResolveType.NoMetaWorkspaces)
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
            var result = _internal.GetComposites(workspaceId, itemUrl);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpGet("api/elements/find_by_searchstring")]
        public ActionResult<OutFindBySearchString> FindBySearchString(string search)
        {
            return _internal.FindBySearchString(search);
        }


        public class OutFindBySearchString
        {
            public const string ResultTypeNone = "none";
            public const string ResultTypeReference = "reference";
            public const string ResultTypeReferenceExtent = "referenceExtent";
            public string resultType { get; set; } = ResultTypeNone;
            public ItemWithNameAndId? reference { get; set; }
        }
    }
}