using DatenMeister.Core.Interfaces.ChangeEvents;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Core.Runtime.ChangeEvents;

/// <summary>
/// Defines the contents functions to be called in case there is a change in the given extent. 
/// </summary>
public class RegisteredEventHandle : EventHandle
{
    /// <summary>
    /// The event handler will only be called in case that value changes
    /// </summary>
    public IObject? Value { get; set; }

    /// <summary>
    /// The event handler will only be called in case that extent changes
    /// </summary>
    public IExtent? Extent { get; set; }

    /// <summary>
    /// The event handler will only be called in case that workspace changes
    /// </summary>
    public IWorkspace? Workspace { get; set; }

    /// <summary>
    /// This is the event handler being called in case a value or its extent or its workspace changes
    /// </summary>
    public Func<IObject?, Task>? ValueAction { get; set; }

    /// <summary>
    /// This is the event handler being called in case an extent or its workspace changes
    /// </summary>
    public Func<IExtent, IObject?, Task>? ExtentAction { get; set; }

    /// <summary>
    /// This is the event handler being called in case an extent or a workspace changes
    /// </summary>
    public Func<IWorkspace, IExtent?, IObject?, Task>? WorkspaceAction { get; set; }
}