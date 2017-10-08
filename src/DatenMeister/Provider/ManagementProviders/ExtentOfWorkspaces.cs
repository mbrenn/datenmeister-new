using System.Collections.Generic;
using System.Linq;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.ManagementProviders
{
    /// <summary>
    /// Contains all workspaces in an extent like structure
    /// </summary>
    public class ExtentOfWorkspaces : IProvider
    {
        /// <summary>
        /// Gets the uri of the extent which contains the workspaces
        /// </summary>
        public const string WorkspaceUri = "datenmeister:///_internal/workspaces/";

        /// <summary>
        /// Initializes a new instance of the ExtentOfWorkspaces
        /// </summary>
        /// <param name="workspaceLogic">Logic of the workspace</param>
        public ExtentOfWorkspaces(IWorkspaceLogic workspaceLogic)
        {
            WorkspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets the workspace logic for the elements
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic { get; }

        public IProviderObject CreateElement(string metaClassUri)
        {
            throw new System.NotImplementedException();
        }

        public void AddElement(IProviderObject valueAsObject, int index = -1)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteElement(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAllElements()
        {
            throw new System.NotImplementedException();
        }

        public IProviderObject Get(string id)
        {
            var result = WorkspaceLogic.GetWorkspace(id);
            if (result == null)
            {
                return null;
            }

            return new WorkspaceObject(this, result);
        }

        public IEnumerable<IProviderObject> GetRootObjects()
        {
            var workspaces = WorkspaceLogic.Workspaces;
            return workspaces.Select(x => new WorkspaceObject(this, x));
        }
    }
}