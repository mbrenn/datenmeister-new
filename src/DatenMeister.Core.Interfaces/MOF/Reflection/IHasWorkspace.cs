using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Core.Interfaces.MOF.Reflection;

/// <summary>
/// Defines that the certain element has a reference to the workspace.
/// </summary>
public interface IHasWorkspace
{
    IWorkspace? Workspace { get; }
}