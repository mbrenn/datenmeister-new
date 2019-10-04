using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public static class Extension
    {
        /// <summary>
        /// Creates an xmi extent and adds it to the Data workspace
        /// </summary>
        /// <param name="extentManager">Extent Manager to be used</param>
        /// <param name="uri"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IUriExtent CreateAndAddXmiExtent(this IExtentManager extentManager, string uri, string filename)
        {
            var xmiConfiguration = new XmiStorageConfiguration
            {
                extentUri = uri,
                workspaceId = WorkspaceNames.NameData,
                filePath = filename
            };

            return extentManager.LoadExtent(xmiConfiguration, ExtentCreationFlags.LoadOrCreate);
        }

        /// <summary>
        /// Loads the extent, if the extent is not already loaded.
        /// If the extent is already loaded, the already loaded extent will be directly returned
        /// If the extent is not loaded, the extent will be loaded according the configuration
        /// </summary>
        /// <param name="extentManager">The extent manager to be used</param>
        /// <param name="loaderConfiguration">The loader configuration being used to load the extent</param>
        /// <param name="flags">The extent creation flags being used to load the extent</param>
        /// <returns>The found or loaded extent</returns>
        public static IUriExtent LoadExtentIfNotAlreadyLoaded(
            this IExtentManager extentManager,
            ExtentLoaderConfig loaderConfiguration,
            ExtentCreationFlags flags = ExtentCreationFlags.LoadOnly)
        {
            var asExtentManager = extentManager as ExtentManager
                                  ?? throw new InvalidOperationException("extentManager is not ExtentManager");
            var workspaceLogic = asExtentManager.WorkspaceLogic;
            var workspace = workspaceLogic.GetWorkspace(loaderConfiguration.workspaceId);
            
            var foundExtent = workspace?.extent.OfType<IUriExtent>().FirstOrDefault(
                x => x.contextURI() == loaderConfiguration.extentUri);
            
            return foundExtent ?? extentManager.LoadExtent(loaderConfiguration, flags);
        }
    }
}