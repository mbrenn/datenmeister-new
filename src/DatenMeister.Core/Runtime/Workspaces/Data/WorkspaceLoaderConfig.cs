namespace DatenMeister.Core.Runtime.Workspaces.Data;

/// <summary>
/// Defines the configuration for the workspace loader
/// </summary>
public class WorkspaceLoaderConfig
{
    public WorkspaceLoaderConfig()
    {
                
    }
        
    public WorkspaceLoaderConfig(string filepath)
    {
        this.filepath = filepath;
    }

    /// <summary>
    /// Sets the file path being used to store the workspaces
    /// </summary>
    public string filepath { get; set; } = string.Empty;
}