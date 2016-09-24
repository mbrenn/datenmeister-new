using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Helper;
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
            if (_fileData.Default == null)
            {
                throw new InvalidOperationException("DataLayer.Default was not set");
            }
        }

        public void SetDefaultDatalayer(Workspace layer)
        {
            lock (_fileData)
            {
                _fileData.Default = layer;
            }
        }

        public void SetRelationShip(Workspace dataLayer, Workspace metaDataLayer)
        {
            lock (_fileData)
            {
                dataLayer.MetaWorkspace = metaDataLayer;
            }
        }

        public void AssignToDataLayer(IExtent extent, Workspace dataLayer)
        {
            lock (_fileData)
            {
                dataLayer.extent.Add(extent);
            }
        }

        public Workspace GetDataLayerOfExtent(IExtent extent)
        {
            lock (_fileData)
            {
                var result = _fileData.Workspaces.FirstOrDefault(x => x.extent.Contains(extent));
                return result ?? _fileData.Default;
            }
        }

        public Workspace GetDataLayerOfObject(IObject value)
        {
            // If the object is contained by another object, query the contained objects
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetDataLayerOfObject(parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IObjectKnowsExtent;
            if (objectKnowsExtent != null)
            {
                var found = objectKnowsExtent.Extents.FirstOrDefault();
                return found == null
                    ? _fileData.Default
                    : GetDataLayerOfExtent(found);
            }

            // Otherwise check it by the dataextent
            lock (_fileData)
            {
                return _fileData.Workspaces.FirstOrDefault(x => x.extent.Cast<IUriExtent>().WithElement(value) != null);
            }
        }

        [Obsolete]
        public Workspace GetMetaLayerFor(Workspace data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            lock (_fileData)
            {
                return data.MetaWorkspace;
            }
        }

        public IEnumerable<IUriExtent> GetExtentsForDatalayer(Workspace dataLayer)
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

        public static Workspaces InitDefault(out WorkspaceData workspace)
        {
            var workspaces = new Workspaces
            {
                Data = new Workspace(Runtime.Workspaces.Workspaces.NameData, "All the data workspaces"),
                Types = new Workspace(Runtime.Workspaces.Workspaces.NameTypes, "All the types belonging to us. "),
                Uml = new Workspace(Runtime.Workspaces.Workspaces.NameUml, "The extents belonging to UML are stored here."),
                Mof = new Workspace(Runtime.Workspaces.Workspaces.NameMof, "The extents belonging to MOF are stored here.")
            };
            var workspaceMgmt = (new Workspace(Runtime.Workspaces.Workspaces.NameManagement, "Management data for DatenMeister"));

            workspace = new WorkspaceData {Default = workspaces.Data};

            var logic = new WorkspaceLogic(workspace);
            logic.AddWorkspace(workspaces.Data);
            logic.AddWorkspace(workspaces.Types);
            logic.AddWorkspace(workspaces.Uml);
            logic.AddWorkspace(workspaces.Mof);
            logic.AddWorkspace(workspaceMgmt);

            logic.SetRelationShip(workspaces.Data, workspaces.Types);
            logic.SetRelationShip(workspaceMgmt, workspaces.Types);
            logic.SetRelationShip(workspaces.Types, workspaces.Uml);
            logic.SetRelationShip(workspaces.Uml, workspaces.Mof);
            logic.SetRelationShip(workspaces.Mof, workspaces.Mof);
            logic.SetDefaultDatalayer(workspaces.Data);

            workspace.Workspaces.Add(workspaces.Mof);
            workspace.Workspaces.Add(workspaces.Uml);
            workspace.Workspaces.Add(workspaces.Types);
            workspace.Workspaces.Add(workspaces.Data);

            return workspaces;
        }

        public static IWorkspaceLogic GetDefaultLogic()
        {
            WorkspaceData data;
            InitDefault(out data);
            return new WorkspaceLogic(data);
        }
    }
}