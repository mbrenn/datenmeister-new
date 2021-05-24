using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class WorkspacesAndExtent : PageModel
    {
        private readonly WorkspaceController _workspaceController;

        public WorkspacesAndExtent(WorkspaceController workspaceController)
        {
            _workspaceController = workspaceController;
        }

        public List<WorkspaceModel> Workspaces = new();
        
        public void OnGet()
        {
            Workspaces = _workspaceController.GetWorkspaceModels();
        }
    }
}