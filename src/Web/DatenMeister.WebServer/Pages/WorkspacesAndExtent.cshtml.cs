using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class WorkspacesAndExtent : PageModel
    {
        private readonly WorkspaceController _workspaceController;

        public List<WorkspaceModel> Workspaces = new();

        public WorkspacesAndExtent(WorkspaceController workspaceController)
        {
            _workspaceController = workspaceController;
        }

        public void OnGet()
        {
            Workspaces = _workspaceController.GetWorkspaceModels();
        }

        public string GetIdOfWorkspace(WorkspaceModel workspaceModel)
        {
            var workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspace(workspaceModel.id)
                            ?? throw new InvalidOperationException(
                                "Workspace is not found");
            return ExtentManagementHelper.GetIdOfWorkspace(workspace);
        }

        public string GetIdOfExtent(WorkspaceModel workspaceModel, ExtentModel extentModel)
        {
            var workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspace(workspaceModel.id)
                            ?? throw new InvalidOperationException("Workspace is not found");
            var extent = workspace.FindExtent(extentModel.uri)
                         ?? throw new InvalidOperationException("Extent is not found");

            return ExtentManagementHelper.GetIdOfExtent(workspace, extent);
        }

        public string GetIdOfExtentsProperties(WorkspaceModel workspaceModel, ExtentModel extentModel)
        {
            var workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspace(workspaceModel.id)
                            ?? throw new InvalidOperationException("Workspace is not found");
            var extent = workspace.FindExtent(extentModel.uri)
                         ?? throw new InvalidOperationException("Extent is not found");

            return ExtentManagementHelper.GetIdOfExtentsProperties(workspace, extent);
        }
    }
}