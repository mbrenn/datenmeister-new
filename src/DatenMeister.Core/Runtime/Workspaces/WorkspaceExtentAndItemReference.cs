// ReSharper disable InconsistentNaming
namespace DatenMeister.Core.Runtime.Workspaces;

/// <summary>
///     This class is used to reference a single object within the database
/// </summary>
public class WorkspaceExtentAndItemReference(string ws, string extent, string item)
{
    public string ws { get; } = ws;
    public string extent { get; } = extent;
    public string item { get; } = item;

    public override string ToString() =>
        $"{ws} - {extent} - {item}";
}