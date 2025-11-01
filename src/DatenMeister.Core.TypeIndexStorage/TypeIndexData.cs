using DatenMeister.Core.TypeIndexAssembly.Model;

namespace DatenMeister.Core.TypeIndexAssembly;

/// <summary>
/// Stores the data of all the types
/// </summary>
public class TypeIndexData
{
    /// <summary>
    /// Stores all Workspaces
    /// </summary>
    public List<WorkspaceModel> Workspaces = new();

    /// <summary>
    /// Finds the workspace by giving the id within the workspace model list
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <returns>The found workspace</returns>
    public WorkspaceModel? FindWorkspace(string workspaceId)
    {
        return Workspaces.FirstOrDefault(x => x.WorkspaceId == workspaceId);
    }

    /// <summary>
    /// Returns, if the workspace is being requested by one of the existing Workspaces
    /// as being a metaclass workspace.
    /// </summary>
    /// <param name="workspace">Workspace which is queried</param>
    /// <returns>true, if the given workspace is a metaclass workspace</returns>
    public bool IsWorkspaceMetaClass(string workspace)
    {
        return Workspaces.Any(
            workspaceModel => workspaceModel.MetaclassWorkspaces.Any(
                x => x == workspace));
    }

    public WorkspaceModel? GetWorkspace(string workspace) 
        => Workspaces.FirstOrDefault(x => x.WorkspaceId == workspace);
}