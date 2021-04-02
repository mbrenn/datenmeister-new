using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Runtime.DynamicFunctions;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// The logic defines the relationships between the layers and the metalayers.
    /// </summary>
    public class WorkspaceLogic : IWorkspaceLogic
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(WorkspaceLogic));

        private readonly WorkspaceData _workspaceData;
        
        private readonly ChangeEventManager? _changeEventManager;

        /// <summary>
        /// Cache to store the uri storing the extents as a provider object
        /// </summary>
        private IUriExtent? _cacheUriExtents;

        private WorkspaceLogic(WorkspaceData workspaceData, IScopeStorage? scopeStorage)
        {
            _workspaceData = workspaceData;
            _changeEventManager = scopeStorage?.Get<ChangeEventManager>();
        }
        
        /// <summary>
        /// Initializes a new instance of the WorkspaceLogic
        /// </summary>
        /// <param name="workspaceData"></param>
        /// <param name="scopeStorage">The scope storage</param>
        public static WorkspaceLogic Create(WorkspaceData workspaceData, IScopeStorage? scopeStorage = null)
        {
            return new WorkspaceLogic(workspaceData, scopeStorage);
        }

        public WorkspaceLogic(IScopeStorage scopeStorage)
        {
            _workspaceData = scopeStorage.Get<WorkspaceData>();
            _changeEventManager = scopeStorage.Get<ChangeEventManager>(); 
        }

        /// <summary>
        /// Sets the default workspace that will be assumed, when the extent is not assigned to a workspace
        /// </summary>
        /// <param name="layer"></param>
        public void SetDefaultWorkspace(Workspace layer)
        {
            lock (_workspaceData)
            {
                _workspaceData.Default = layer;
            }
        }

        public Workspace? GetWorkspaceOfExtent(IExtent? extent)
        {
            lock (_workspaceData)
            {
                if (extent == null) return _workspaceData.Default;
                
                var result = _workspaceData.Workspaces.FirstOrDefault(x => x.extent.Contains(extent));
                return result ?? _workspaceData.Default;
            }
        }

        /// <summary>
        /// Gets the workspace of the extent which is storing the object
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>The found workspace or the default one, if no extent was found</returns>
        public Workspace? GetWorkspaceOfObject(IObject value)
        {
            // If the object is contained by another object, query the contained objects
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetWorkspaceOfObject(parent);
            }

            Workspace? result = null;
            // If the object knows the extent to which it belongs to, it will return it
            if (value is IHasExtent objectKnowsExtent)
            {
                var found = objectKnowsExtent.Extent;
                result = GetWorkspaceOfExtent(found);
            }

            lock (_workspaceData)
            {
                // Otherwise check it by the data extent
                if (result != null)
                {
                    result = _workspaceData.Workspaces.FirstOrDefault(x =>
                        x.extent.Cast<IUriExtent>().WithElement(value) != null);
                }

                return result ?? _workspaceData.Default;
            }
        }

        public IEnumerable<IUriExtent> GetExtentsForWorkspace(Workspace dataLayer)
        {
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            lock (_workspaceData)
            {
                return dataLayer.extent
                    .Select(x => x as IUriExtent)
                    .Where(x => x != null)
                    .ToList()!;
            }
        }

        /// <summary>
        /// Gets the datalayer by name.
        /// The datalayer will only be returned, if there is a relationship
        /// </summary>
        /// <param name="id">Name of the datalayer</param>
        /// <returns>Found datalayer or null</returns>
        public Workspace GetById(string id)
        {
            lock (_workspaceData)
            {
                return _workspaceData.Workspaces.First(x => x.id == id);
            }
        }

        /// <summary>
        /// Gets the default workspace
        /// </summary>
        /// <returns>The default workspace</returns>
        public Workspace? GetDefaultWorkspace()
        {
            lock (_workspaceData)
            {
                return _workspaceData.Default;
            }
        }

        /// <summary>
        /// Adds a workspace and assigns the meta workspace for the given workspace
        /// The meta workspace can also be the same as the added workspace
        /// </summary>
        /// <param name="workspace">Workspace to be added</param>
        public Workspace AddWorkspace(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));

            lock (_workspaceData)
            {
                // Check, if id of workspace is already known
                if (_workspaceData.Workspaces.Any(x => x.id == workspace.id))
                {
                    throw new InvalidOperationException("id is already known");
                }

                Logger.Debug($"Adding workspace {workspace.id}");
                _workspaceData.Workspaces.Add(workspace);

                // If no metaworkspace is given, define the one from the default one
                var metaWorkspaces = _workspaceData.Default?.MetaWorkspaces;
                if (workspace.MetaWorkspaces.Count == 0 && metaWorkspaces != null)
                {
                    foreach (var innerWorkspace in metaWorkspaces)
                    {
                        workspace.AddMetaWorkspace(innerWorkspace);
                    }
                }
            }

            SendEventForWorkspaceChange(workspace);

            return workspace;
        }

        /// <summary>
        /// Sends an event for a workspace change
        /// </summary>
        /// <param name="workspace">The workspace that has been changed.</param>
        public void SendEventForWorkspaceChange(Workspace workspace)
        {
            if (workspace != null)
            {
                _changeEventManager?.SendChangeEvent((IWorkspace) workspace);
            }

            _cacheUriExtents ??= this.TryGetManagementWorkspace()?.FindExtent(WorkspaceNames.UriExtentWorkspaces);

            if (_cacheUriExtents != null)
            {
                _changeEventManager?.SendChangeEvent(_cacheUriExtents);
            }
        }

        /// <summary>
        /// Gets the workspace by id
        /// </summary>
        /// <param name="id">Id of the workspace</param>
        /// <returns>Found the workspace</returns>
        public Workspace? GetWorkspace(string id)
        {
            lock (_workspaceData)
            {
                return _workspaceData.Workspaces.FirstOrDefault(x => x.id == id);
            }
        }

        public DynamicFunctionManager GetDynamicFunctionManager(string workspaceId)
        {
                return (GetWorkspace(workspaceId) 
                        ?? throw new InvalidOperationException($"Workspace not found {workspaceId}"))
                    .DynamicFunctionManager;
        }

        /// <summary>
        /// Gets an enumeration of all workspaces
        /// </summary>
        public IEnumerable<Workspace> Workspaces
        {
            get
            {
                lock (_workspaceData)
                {
                    return _workspaceData.Workspaces.ToList();
                }
            }
        }

        /// <summary>
        /// Removes the workspace from the collection
        /// </summary>
        /// <param name="id">Id of the workspace to be deleted</param>
        public void RemoveWorkspace(string id)
        {
            Workspace? workspaceToBeDeleted;
            lock (_workspaceData)
            {
                workspaceToBeDeleted = GetWorkspace(id);

                if (workspaceToBeDeleted != null)
                {
                    _workspaceData.Workspaces.Remove(workspaceToBeDeleted);
                }
            }

            if (workspaceToBeDeleted != null)
            {
                SendEventForWorkspaceChange(workspaceToBeDeleted);
            }
        }

        /// <summary>
        /// Adds an extent to the workspace
        /// </summary>
        /// <param name="workspace">Workspace to which the extent shall be added</param>
        /// <param name="newExtent">The extent to be added</param>
        public void AddExtent(Workspace workspace, IUriExtent newExtent)
        {
            workspace.AddExtent(newExtent);
            if (newExtent is MofExtent mofExtent
                && mofExtent.ChangeEventManager != _changeEventManager)
            {
                mofExtent.ChangeEventManager = _changeEventManager;
            }

            if (newExtent is MofUriExtent mofUriExtent)
            {
                lock (_workspaceData)
                {
                    mofUriExtent.DynamicFunctionManager = workspace.DynamicFunctionManager;
                }
            }

            SendEventForWorkspaceChange(workspace);
        }

        /// <summary>
        /// Performs an initialization of the common workspaces
        /// </summary>
        /// <returns>The data containing the workspaces</returns>
        public static WorkspaceData InitDefault()
        {
            var workspaceData = new Workspace(WorkspaceNames.WorkspaceData, "All the data workspaces");
            var workspaceTypes = new Workspace(WorkspaceNames.WorkspaceTypes, "All the types belonging to us. ");
            var workspaceUml = new Workspace(WorkspaceNames.WorkspaceUml, "The extents belonging to UML are stored here.");
            var workspaceMof = new Workspace(WorkspaceNames.WorkspaceMof, "The extents belonging to MOF are stored here.");
            var workspaceMgmt = new Workspace(WorkspaceNames.WorkspaceManagement, "Management data for DatenMeister");

            var workspace = new WorkspaceData {Default = workspaceData};

            var logic = Create(workspace);
            logic.AddWorkspace(workspaceData);
            logic.AddWorkspace(workspaceTypes);
            logic.AddWorkspace(workspaceUml);
            logic.AddWorkspace(workspaceMof);
            logic.AddWorkspace(workspaceMgmt);

            workspaceData.AddMetaWorkspace(workspaceTypes);
            workspaceMgmt.AddMetaWorkspace(workspaceTypes);
            workspaceTypes.AddMetaWorkspace(workspaceUml);
            workspaceUml.AddMetaWorkspace(workspaceMof);
            workspaceMof.AddMetaWorkspace(workspaceMof);
            logic.SetDefaultWorkspace(workspaceData);

            return workspace;
        }

        public static IWorkspaceLogic GetDefaultLogic() => Create(InitDefault());

        public static IWorkspaceLogic GetEmptyLogic() => Create(new WorkspaceData());
    }
}