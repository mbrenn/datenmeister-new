using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Defines the core being used as the central connection point
    /// </summary>
    public class WorkspaceCollection : IWorkspaceCollection
    {
        public class Item
        {
            public Workspace<IExtent> Workspace { get; set; }
        }

        /// <summary>
        ///     Stores the workspace for all extents
        /// </summary>
        private List<Item> _workspaces = new List<Item>();
        
        /// <summary>
        /// Gets all the workspaces
        /// </summary>
        public IEnumerable<Workspace<IExtent>> Workspaces
        {
            get
            {
                lock (_workspaces)
                {
                    return _workspaces.Select(x=>x.Workspace).ToList();
                }
            }
        }

        /// <summary>
        /// Adds a workspace and assigns the meta workspace for the given workspace
        /// The meta workspace can also be the same as the added workspace
        /// </summary>
        /// <param name="workspace">Workspace to be added</param>
        /// <param name="metaWorkspace">The meta workspace</param>
        public void AddWorkspace(Workspace<IExtent> workspace)
        {
            if (workspace == null)  throw new ArgumentNullException(nameof(workspace));

            lock (_workspaces)
            {
                    // Check, if id of workspace is already known
                if (_workspaces.Any(x => x.Workspace.id == workspace.id))
                {
                    throw new InvalidOperationException("id is already known");
                }

                _workspaces.Add(
                    new Item()
                    {
                        Workspace = workspace
                    });
            }
        }

        public Item GetWorkspaceItem(string id)
        {
            lock (_workspaces)
            {
                return _workspaces.FirstOrDefault(x => x.Workspace.id == id);
            }
        }

        public Workspace<IExtent> GetWorkspace(string id) => GetWorkspaceItem(id)?.Workspace;

        /// <summary>
        /// Removes the workspace from the collection
        /// </summary>
        /// <param name="id">Id of the workspace to be deleted</param>
        public void RemoveWorkspace(string id)
        {
            lock (_workspaces)
            {
                var workspaceToBeDeleted = GetWorkspaceItem(id);

                if (workspaceToBeDeleted != null)
                {
                    _workspaces.Remove(workspaceToBeDeleted);
                }
            }
        }

        /// <summary>
        ///     Initializes the information
        /// </summary>
        public void Init()
        {
            _workspaces = new List<Item>();
            AddWorkspace(new Workspace<IExtent>("Data", "All the data workspaces"));
            AddWorkspace(new Workspace<IExtent>("Meta", "The meta information. "));
            Debug.WriteLine("DatenMeister Webcore initialized");
        }
    }
}