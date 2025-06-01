// ReSharper disable InconsistentNaming
namespace DatenMeister.Core.Runtime.Workspaces.Data;

/// <summary>
/// Stores the information for a workspace which can be stored as an xml file
/// </summary>
public class WorkspaceInfo(string id, string annotation)
{
    public WorkspaceInfo() : this(string.Empty, string.Empty)
    {
    }

    public string id { get; set; } = id;
    public string annotation { get; set; } = annotation;
}