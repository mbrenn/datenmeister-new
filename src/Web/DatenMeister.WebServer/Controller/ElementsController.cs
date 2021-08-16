
using System;
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
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

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
        public ActionResult<ItemWithNameAndId[]> GetComposites(string? workspaceId = "", string? itemUrl = "")
        {
            if (workspaceId == null)
            {
            }

            throw new InvalidOperationException();
        }
    }
}