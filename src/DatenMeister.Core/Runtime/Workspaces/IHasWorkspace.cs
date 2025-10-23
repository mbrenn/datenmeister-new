using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Core.Runtime.Workspaces;

/// <summary>
/// Defines that the certain element has a reference to the workspace.
/// </summary>
public interface IHasWorkspace
{
    IWorkspace? Workspace { get; }
}