using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Provider.InMemory;
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
        public static ExtentStorageData.LoadedExtentInformation CreateAndAddXmiExtent(this ExtentManager extentManager, string uri, string filename)
        {
            var configuration = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, uri);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId, WorkspaceNames.WorkspaceData);
            configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, filename);

            return extentManager.LoadExtent(configuration, ExtentCreationFlags.LoadOrCreate);
        }

        /// <summary>
        /// Loads the extent, if the extent is not already loaded.
        /// If the extent is already loaded, the already loaded extent will be directly returned
        /// If the extent is not loaded, the extent will be loaded according the configuration
        /// </summary>
        /// <param name="extentManager">The extent manager to be used</param>
        /// <param name="loaderConfiguration">The loader configuration being used to load the extent
        /// Of Type ExtentLoaderConfig</param>
        /// <param name="flags">The extent creation flags being used to load the extent</param>
        /// <returns>The found or loaded extent</returns>
        public static IUriExtent? LoadExtentIfNotAlreadyLoaded(
            this ExtentManager extentManager,
            IElement loaderConfiguration,
            ExtentCreationFlags flags = ExtentCreationFlags.LoadOnly)
        {
            var asExtentManager = extentManager
                                  ?? throw new InvalidOperationException("extentManager is not ExtentManager");
            var workspaceLogic = asExtentManager.WorkspaceLogic;
            var workspace = workspaceLogic.GetWorkspace(
                loaderConfiguration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig
                    .workspaceId));

            var foundExtent = workspace?.extent.OfType<IUriExtent>().FirstOrDefault(
                x => x.contextURI() ==
                     loaderConfiguration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig
                         .extentUri));

            return foundExtent ?? extentManager.LoadExtent(loaderConfiguration, flags).Extent;
        }

        /// <summary>
        /// Deletes the extent from the workspace and the extent manager
        /// </summary>
        /// <param name="manager">Manager to be used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <returns>true, if successfully deleted</returns>
        public static bool DeleteExtent(this ExtentManager manager, string workspaceId, string extentUri)
        {
            var found = manager.WorkspaceLogic.FindExtent(workspaceId, extentUri);
            if (found != null)
            {
                manager.RemoveExtent(found);
                return true;
            }

            return false;
        }
    }
}