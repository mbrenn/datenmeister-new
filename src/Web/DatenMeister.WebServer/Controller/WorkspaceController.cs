using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public WorkspaceController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public class DeleteWorkspaceParams
        {
            public string Workspace { get; set; } = string.Empty;
        }
        
        [HttpDelete("api/workpsace/delete")]
        public ActionResult<object> DeleteWorkspace([FromBody] DeleteWorkspaceParams workspace)
        {
            _workspaceLogic.RemoveWorkspace(workspace.Workspace);
            return new {success = true};
        }
    }
}