using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.WebServer.Models;

namespace DatenMeister.WebServer.InterfaceController;

/// <summary>
/// This controller supports the export of the workspaces including extents
/// </summary>
public class WorkspaceController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    private readonly IScopeStorage _scopeStorage = scopeStorage;

    public List<WorkspaceModel> GetWorkspaceModels()
    {
        var result = new List<WorkspaceModel>();
        var workspaces = workspaceLogic.Workspaces;
        foreach (var workspace in workspaces)
        {
            var workspaceModel = new WorkspaceModel
            {
                id = workspace.id, 
                annotation = workspace.annotation
            };

            foreach (var extent in workspace.extent.OfType<IUriExtent>())
            {
                var extentModel = new ExtentModel
                {
                    uri = extent.contextURI()
                };
                    
                workspaceModel.extents.Add(extentModel);
            }

            result.Add(workspaceModel);
        }

        return result;
    }
}