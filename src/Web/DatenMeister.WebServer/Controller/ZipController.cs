using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Modules.ZipCodeExample;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/zip")]
    [ApiController]
    public class ZipController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public ZipController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }
        
        [HttpPost("api/zip/create")]
        public ActionResult<object> CreateZipExample([FromForm] string workspace)
        {
            var zipExample = new ZipCodeExampleManager(_workspaceLogic,
                new ExtentManager(_workspaceLogic, _scopeStorage), _scopeStorage);
            zipExample.AddZipCodeExample(workspace);

            return new {success = true};
        }
    }
}