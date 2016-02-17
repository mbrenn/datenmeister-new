using System.Collections;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Defines the interface which stores a set of workspaces
    /// </summary>
    public interface IWorkspaceCollection
    {
        void AddWorkspace(Workspace<IExtent> workspace);
        void RemoveWorkspace(string id);

        Workspace<IExtent> GetWorkspace(string id);
        IEnumerable<Workspace<IExtent>> Workspaces { get; }
    }
}