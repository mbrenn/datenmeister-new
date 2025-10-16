using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Runtime.ChangeEvents;

public class RegisteredEventHandle : EventHandle
{
    public IObject? Value { get; set; }
    public IExtent? Extent { get; set; }
    public IWorkspace? Workspace { get; set; }
    public Action<IObject?>? ValueAction { get; set; }
    public Action<IExtent, IObject?>? ExtentAction { get; set; }
    public Action<IWorkspace, IExtent?, IObject?>? WorkspaceAction { get; set; }
}