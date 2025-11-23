using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Core.Interfaces.ChangeEvents;

/// <summary>
/// Provides an interface for managing and handling change events associated with objects,
/// extents, and workspaces. Allows for registration and unregistration of event listeners
/// that react to changes, insertions, or deletions in the observed entities.
/// </summary>
public interface IChangeEventManager
{
    EventHandle RegisterFor(IObject value, Func<IObject?, Task> valueAction);
    
    EventHandle RegisterFor(IExtent extent, Func<IExtent, IObject?, Task> extentAction);
    
    EventHandle RegisterFor(IWorkspace workspace, Func<IWorkspace, IExtent?, IObject?, Task> workspaceAction);
    
    void Unregister(EventHandle eventHandle);
}