using System.Diagnostics;
using System.IO;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Implements some helper methods to create extents
    /// </summary>
    public class ExtentCreator
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentManager _extentManager;

        public ExtentCreator(IWorkspaceLogic workspaceLogic, ExtentManager extentManager)
        {
            _workspaceLogic = workspaceLogic;
            _extentManager = extentManager;
        }

        /// <summary>
        /// Gets or creates an Xmi Extent in the internal database
        /// </summary>
        /// <param name="scope">Dependency Container</param>
        /// <param name="workspace">Workspace in which the Xmi extent will be stored</param>
        /// <param name="uri">Uri of the extent</param>
        /// <param name="name">Name of the extent (being used to stored into database). The
        /// name needs to be unique</param>
        /// <returns>The uri extent being found</returns>
        public static IUriExtent GetOrCreateXmiExtentInInternalDatabase(ILifetimeScope scope, string workspace,
            string uri, string name)
        {
            var creator = scope.Resolve<ExtentCreator>();
            return creator.GetOrCreateXmiExtentInInternalDatabase(workspace, uri, name);
        }

        public IUriExtent GetOrCreateXmiExtentInInternalDatabase(string workspace, string uri, string name)
        { 
            // Creates the user types, if not existing
            var foundExtent = _workspaceLogic.FindExtent(uri);
            if (foundExtent == null)
            {
                Debug.WriteLine("Creates the extent for the user types");
                // Creates the extent for user types
                var storageConfiguration = new XmiStorageConfiguration
                {
                    ExtentUri = uri,
                    Path = Path.Combine("App_Data/Database", name + ".xml"),
                    Workspace = workspace,
                    DataLayer = "Types"
                };

                foundExtent = _extentManager.LoadExtent(storageConfiguration, true);
                return (IUriExtent) foundExtent;
            }

            return (IUriExtent) foundExtent;
        }

    }
}