namespace DatenMeister.Core.TypeIndexAssembly.Model;

/// <summary>
/// Stores the dependencies of the workspace themselves. This is used to have a fast access
/// to the Metaclass Workspaces
/// </summary>
public class WorkspaceModel
{
    /// <summary>
    /// Id of the workspace
    /// </summary>
    public string WorkspaceId { get; set; } = string.Empty;

    /// <summary>
    /// Stores the list of workspaces which may contain the types of the metaclasses
    /// </summary>
    public List<string> MetaclassWorkspaces { get; } = new();

    /// <summary>
    /// Stores all the class models for the models
    /// </summary>
    public List<ClassModel> ClassModels { get; } = new();
}