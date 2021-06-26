using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        
        public class DeleteWorkspaceParams
        {
            public string Workspace { get; set; } = string.Empty;
        }
        
        [HttpGet("api/workpsace/delete")]
        public void DeleteWorkspace([FromForm] string workspace)
        {
            
        }
    }
}