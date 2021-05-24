
using System.Threading.Tasks;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
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

        [HttpGet("api/elements/get_name/{uri}")]
        public ActionResult<object> GetName(string uri)
        {
            var foundItem = _workspaceLogic.FindItem(HttpUtility.UrlDecode(uri));
            if (foundItem == null)
            {
                return NotFound();
            }

            return new {name = NamedElementMethods.GetName(foundItem)};
        }
    }
}