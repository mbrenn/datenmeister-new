using System;
using System.Collections.Generic;
using System.Linq;
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

        public WorkspaceLogic(WorkspaceData fileData)
        {
            _fileData = fileData;
        }

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

            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IHasExtent;
            if (objectKnowsExtent != null)
            {
                var found = objectKnowsExtent.Extent;
                return found == null
                    ? _fileData.Default
                    : GetWorkspaceOfExtent(found);
            }

            // Otherwise check it by the dataextent
            lock (_fileData)
            {
                return _fileData.Workspaces.FirstOrDefault(x => x.extent.Cast<IUriExtent>().WithElement(value) != null);
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
            return _fileData.Default;
        }

        /// <summary>
        /// Adds a workspace and assigns the meta workspace for the given workspace
        /// The meta workspace can also be the same as the added workspace
        /// </summary>
        /// <param name="workspace">Workspace to be added</param>
        public Workspace AddWorkspace(Workspace workspace)
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
                if (workspace.MetaWorkspace == null)
                {
                    workspace.MetaWorkspace = _fileData.Default?.MetaWorkspace;
                }
            }

            return workspace;
        }

        public Workspace GetWorkspace(string id)
        {
            lock (_fileData)
            {
                return _fileData.Workspaces.FirstOrDefault(x => x.id == id);
            }
        }

        public IEnumerable<Workspace> Workspaces => _fileData.Workspaces.ToList();

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

            workspaceData.MetaWorkspace = workspaceTypes;
            workspaceMgmt.MetaWorkspace = workspaceTypes;
            workspaceTypes.MetaWorkspace = workspaceUml;
            workspaceUml.MetaWorkspace = workspaceMof;
            workspaceMof.MetaWorkspace = workspaceMof;
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