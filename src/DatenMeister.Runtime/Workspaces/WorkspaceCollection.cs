using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;

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
                    new Item
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

        /// <summary>
        /// Gets a workspace by the given id
        /// </summary>
        /// <param name="id">Id of the workspace</param>
        /// <returns>The found workspace or null, if not found</returns>
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
            AddWorkspace(new Workspace<IExtent>(WorkspaceNames.Data, "All the data workspaces"));
            AddWorkspace(new Workspace<IExtent>(WorkspaceNames.Management, "Management data for DatenMeister"));
            AddWorkspace(new Workspace<IExtent>(WorkspaceNames.Types, "All the types belonging to us. "));
            AddWorkspace(new Workspace<IExtent>(WorkspaceNames.Uml, "The extents belonging to UML are stored here."));
            AddWorkspace(new Workspace<IExtent>(WorkspaceNames.Mof, "The extents belonging to MOF are stored here."));
            Debug.WriteLine("DatenMeister Webcore initialized");
        }
    }
}