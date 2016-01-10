using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.Workspaces
{
    public interface IWorkspaceCollection
    {
        void AddWorkspace(Workspace<IExtent> workspace);
        Workspace<IExtent> GetWorkspace(string id);
        IEnumerable<Workspace<IExtent>> Workspaces { get; }
    }
}