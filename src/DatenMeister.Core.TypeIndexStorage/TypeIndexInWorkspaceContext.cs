using DatenMeister.Core.TypeIndexAssembly.Model;

namespace DatenMeister.Core.TypeIndexAssembly;

/// <summary>
/// Supports a more easy access to the type index within the context of a workspace.
/// This class can be used by the IProvider instances to find the class models providing a certain access
/// </summary>
/// <param name="logic">Logic which is queried</param>
/// <param name="workspace">The workspace in which the context is residing</param>
public class TypeIndexInWorkspaceContext(TypeIndexLogic logic, string workspace)
{
    /// <summary>
    /// Gets or sets the workspace in which the context is residing.
    /// </summary>
    public string Workspace { get; set; } = workspace;
    
    /// <summary>
    /// Gets or sets the logic which is queried.
    /// </summary>
    public TypeIndexLogic Logic { get; set; } = logic;
    
    /// <summary>
    /// Finds the class model within the metaclass workspaces of the set workspace
    /// </summary>
    /// <param name="uri">Uri of the class being requested</param>
    /// <returns>The found classmodel or null, if not found</returns>
    public ClassModel? FindClassModel(string uri)
    {
        return Logic.FindClassModelByUrl(Workspace, uri);
    }
}