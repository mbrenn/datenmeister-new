// ReSharper disable InconsistentNaming
namespace DatenMeister.Core.Runtime.Workspaces;

/// <summary>
///     This class is used to reference a single object within the database
/// </summary>
public class WorkspaceExtentAndItemReference
{
    public string ws { get; }
    public string extent { get; }
    public string item { get; }

    public WorkspaceExtentAndItemReference(string ws, string extent, string item)
    {
        this.ws = ws;
        this.extent = extent;
        this.item = item;
    }

    public override string ToString() =>
        $"{ws} - {extent} - {item}";
}