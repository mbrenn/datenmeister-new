using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Defines the interface which stores a set of workspaces
    /// </summary>
    public interface IWorkspaceCollection
    {
        void AddWorkspace(Workspace workspace);

        void RemoveWorkspace(string id);

        Workspace GetWorkspace(string id);
        IEnumerable<Workspace> Workspaces { get; }
    }
}