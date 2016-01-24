using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    ///     Defines the core being used as the central connection point
    /// </summary>
    public class WorkspaceCollection : IWorkspaceCollection
    {
        /// <summary>
        ///     Stores the workspace for all extents
        /// </summary>
        private List<Workspace<IExtent>> _workspaces = new List<Workspace<IExtent>>();
        
        /// <summary>
        /// Gets all the workspaces
        /// </summary>
        public IEnumerable<Workspace<IExtent>> Workspaces
        {
            get
            {
                lock (_workspaces)
                {
                    return _workspaces.ToList();
                }
            }
        }

        public void AddWorkspace(Workspace<IExtent> workspace)
        {
            lock (_workspaces)
            {
                // Check, if id of workspace is already known
                if (_workspaces.Any(x => x.id == workspace.id))
                {
                    throw new InvalidOperationException("id is already known");
                }

                _workspaces.Add(workspace);
            }
        }

        public Workspace<IExtent> GetWorkspace(string id)
        {
            lock (_workspaces)
            {
                return _workspaces.FirstOrDefault(x => x.id == id);
            }
        } 

        /// <summary>
        ///     Initializes the information
        /// </summary>
        public void Init()
        {
            _workspaces = new List<Workspace<IExtent>>();
            AddWorkspace(new Workspace<IExtent>("Data", "All the data workspaces"));
            Debug.WriteLine("DatenMeister Webcore initialized");
        }
    }
}