using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// The logic defines the relationships between the layers and the metalayers.
    /// </summary>
    public class WorkspaceLogic : IWorkspaceLogic
    {
        private readonly WorkspaceData _fileData;

        /// <summary>
        /// Initializes a new instance of the WorkspaceLogic
        /// </summary>
        /// <param name="fileData"></param>
        public WorkspaceLogic(WorkspaceData fileData)
        {
            _fileData = fileData;
        }

        /// <summary>
        /// Sets the default workspace that will be assumed, when the extent is not assigned to a workspace
        /// </summary>
        /// <param name="layer"></param>
        public void SetDefaultWorkspace(Workspace layer)
        {
            lock (_fileData)
            {
                _fileData.Default = layer;
            }
        }

        public Workspace GetWorkspaceOfExtent(IExtent extent)
        {
            lock (_fileData)
            {
                var result = _fileData.Workspaces.FirstOrDefault(x => x.extent.Contains(extent));
                return result ?? _fileData.Default;
            }
        }

        /// <summary>
        /// Gets the workspace of the extent which is storing the object
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>The found workspace or the default one, if no extent was found</returns>
        public Workspace GetWorkspaceOfObject(IObject value)
        {
            // If the object is contained by another object, query the contained objects
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetWorkspaceOfObject(parent);
            }

            Workspace result = null;
            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IHasExtent;
            if (objectKnowsExtent != null)
            {
                var found = objectKnowsExtent.Extent;
                result = GetWorkspaceOfExtent(found);
            }

            lock (_fileData)
            {
                // Otherwise check it by the dataextent
                if (result != null)
                {
                    result = _fileData.Workspaces.FirstOrDefault(x =>
                        x.extent.Cast<IUriExtent>().WithElement(value) != null);
                }

                return result ?? _fileData.Default;
            }
        }

        public IEnumerable<IUriExtent> GetExtentsForWorkspace(Workspace dataLayer)
        {
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            lock (_fileData)
            {
                return dataLayer.extent
                    .Select(x => x as IUriExtent)
                    .Where(x => x != null)
                    .ToList();
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
            lock (_fileData)
            {
                return _fileData.Workspaces.FirstOrDefault(x => x.id == id);
            }
        }

        public Workspace GetDefaultWorkspace()
        {
            lock (_fileData)
            {
                return _fileData.Default;
            }
        }

        /// <summary>
        /// Adds a workspace and assigns the meta workspace for the given workspace
        /// The meta workspace can also be the same as the added workspace
        /// </summary>
        /// <param name="workspace">Workspace to be added</param>
        public void AddWorkspace(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException(nameof(workspace));

            lock (_fileData)
            {
                // Check, if id of workspace is already known
                if (_fileData.Workspaces.Any(x => x.id == workspace.id))
                {
                    throw new InvalidOperationException("id is already known");
                }

                _fileData.Workspaces.Add(workspace);

                // If no metaworkspace is given, define the one from the default one
                if (workspace.MetaWorkspaces.Count == 0 && _fileData.Default?.MetaWorkspaces != null)
                {
                    foreach (var innerWorkspace in _fileData.Default?.MetaWorkspaces)
                    {
                        workspace.AddMetaWorkspace(innerWorkspace);
                    }
                }
            }
        }

        public Workspace GetWorkspace(string id)
        {
            lock (_fileData)
            {
                return _fileData.Workspaces.FirstOrDefault(x => x.id == id);
            }
        }

        /// <summary>
        /// Gets an enumeration of all workspaces
        /// </summary>
        public IEnumerable<Workspace> Workspaces
        {
            get
            {
                lock (_fileData)
                {
                    return _fileData.Workspaces.ToList();
                }
            }
        }

        /// <summary>
        /// Removes the workspace from the collection
        /// </summary>
        /// <param name="id">Id of the workspace to be deleted</param>
        public void RemoveWorkspace(string id)
        {
            lock (_fileData)
            {
                var workspaceToBeDeleted = GetWorkspace(id);

                if (workspaceToBeDeleted != null)
                {
                    _fileData.Workspaces.Remove(workspaceToBeDeleted);
                }
            }
        }

        /// <summary>
        /// Adds an extent to the workspace
        /// </summary>
        /// <param name="workspace">Workspace to which the extent shall be added</param>
        /// <param name="newExtent">The extent to be added</param>
        public void AddExtent(Workspace workspace, IUriExtent newExtent)
        {
            var asMofExtent = (MofExtent) newExtent;
            if (newExtent == null) throw new ArgumentNullException(nameof(newExtent));
            if (asMofExtent.Workspace != null)
            {
                throw new InvalidOperationException("The extent is already assigned to a workspace");
            }

            lock (workspace.SyncObject)
            {
                if (workspace.extent.Any(x => (x as IUriExtent)?.contextURI() == newExtent.contextURI()))
                {
                    throw new InvalidOperationException($"Extent with uri {newExtent.contextURI()} is already added to the given workspace");
                }

                workspace.extent.Add(newExtent);
                asMofExtent.Workspace = workspace;
                ((MofExtent) newExtent).Resolver = new WorkspaceUriResolver(this);
            }
        }

        /// <summary>
        /// Performs an initialization of the common workspaces
        /// </summary>
        /// <returns>The data containing the workspaces</returns>
        public static WorkspaceData InitDefault()
        {
            var workspaceData = new Workspace(WorkspaceNames.NameData, "All the data workspaces");
            var workspaceTypes = new Workspace(WorkspaceNames.NameTypes, "All the types belonging to us. ");
            var workspaceUml = new Workspace(WorkspaceNames.NameUml, "The extents belonging to UML are stored here.");
            var workspaceMof = new Workspace(WorkspaceNames.NameMof, "The extents belonging to MOF are stored here.");
            var workspaceMgmt = new Workspace(WorkspaceNames.NameManagement, "Management data for DatenMeister");

            var workspace = new WorkspaceData {Default = workspaceData};

            var logic = new WorkspaceLogic(workspace);
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

        public static IWorkspaceLogic GetDefaultLogic()
        {
            return new WorkspaceLogic(InitDefault());
        }

        public static IWorkspaceLogic GetEmptyLogic()
        {
            return new WorkspaceLogic(new WorkspaceData());
        }
    }
}