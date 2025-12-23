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
    public readonly List<WorkspaceModel> Workspaces = [];

    /// <summary>
    /// Stores the index of the class models by their URI
    /// </summary>
    private Dictionary<string, ClassModel>? _classModelsIndex;

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

    /// <summary>
    /// Creates the index for all the class models
    /// </summary>
    public void CreateIndex()
    {
        _classModelsIndex = new Dictionary<string, ClassModel>();
        foreach (var workspace in Workspaces)
        {
            foreach (var classModel in workspace.ClassModels)
            {
                if (!string.IsNullOrEmpty(classModel.Uri))
                {
                    _classModelsIndex[classModel.Uri] = classModel;
                }
            }
        }
    }

    /// <summary>
    /// Finds a class model by its URI across all workspaces.
    /// This uses the index for faster lookups if available.
    /// </summary>
    /// <param name="uri">URI of the class model</param>
    /// <returns>The found class model or null</returns>
    public ClassModel? FindClassModelByUri(string uri)
    {
        if (_classModelsIndex != null)
        {
            return _classModelsIndex.GetValueOrDefault(uri);
        }

        foreach (var workspace in Workspaces)
        {
            var found = workspace.FindClassByUri(uri);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}