using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller;

[Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
[ApiController]
public class WorkspaceController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : ControllerBase
{
    private static readonly ClassLogger Logger = new(typeof(WorkspaceController));

    private readonly IScopeStorage _scopeStorage = scopeStorage;

    [HttpDelete("api/workspace/delete")]
    public ActionResult<object> DeleteWorkspace([FromBody] DeleteWorkspaceParams workspace)
    {
        workspaceLogic.RemoveWorkspace(workspace.Id);
        return new {success = true};
    }

    public class DeleteWorkspaceParams
    {
        public string Id { get; set; } = string.Empty;
    }
        
    [HttpPut("api/workspace/create")]
    public ActionResult<object> CreateWorkspace([FromBody] CreateWorkspaceParams workspace)
    {
        try
        {
            workspaceLogic.AddWorkspace(new Workspace(workspace.id, workspace.annotation));

            return new {success = true};
        }
        catch (Exception exc)
        {
            Logger.Error(exc.Message);
            return new {success = false};
        }
            
    }

    public class CreateWorkspaceParams
    {
        public string id { get; set; } = string.Empty;
            
        public string annotation { get; set; } = string.Empty;
    }
}