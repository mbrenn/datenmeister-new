using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.ChangeEvents
{
    public class RegisteredEventHandle : EventHandle
    {
        public IObject? Value { get; set; }
        public IExtent? Extent { get; set; }
        public IWorkspace? Workspace { get; set; }
        public Action<IObject>? ValueAction { get; set; }
        public Action<IExtent, IObject>? ExtentAction { get; set; }
        public Action<IWorkspace, IExtent, IObject>? WorkspaceAction { get; set; }
    }
}