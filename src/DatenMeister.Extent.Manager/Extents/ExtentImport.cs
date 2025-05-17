using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Exceptions;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Extent.Manager.Extents
{
    public class ExtentImport
    {
        private readonly ExtentManager _extentManager;

        public ExtentImport(ExtentManager extentManager)
        {
            _extentManager = extentManager;
        }

        /// <summary>
        /// Imports the file for according the given settings.
        /// The settings are of type "DatenMeister.UserFormSupport.LoadFileFromXmi"
        /// </summary>
        /// <param name="mofImportSettings">Import settings being used</param>
        /// <returns>The created uri extent</returns>
        public async Task<IUriExtent> ImportExtent(IObject mofImportSettings)
        {
            var extentUri = mofImportSettings.getOrDefault<string>("extentUri");
            var workspaceId = mofImportSettings.getOrDefault<string>("workspace");
            var filePath = mofImportSettings.getOrDefault<string>("filePath");
            var pathExtension = Path.GetExtension(filePath).ToLower();
            if (pathExtension == ".xmi" || pathExtension == ".xml")
            {
                var configuration =
                    InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri, extentUri);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath, filePath);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId, workspaceId);
                
                var resultingExtent = await _extentManager.LoadExtent(configuration);

                if (resultingExtent.LoadingState == ExtentLoadingState.Failed)
                {
                    var exception = InMemoryObject.CreateEmpty("#DatenMeister.Models.ExtentManager.ImportException");
                    exception.set("message", "Loading did not succeed");
                    throw new DatenMeisterException(exception);
                }

                return resultingExtent.Extent ?? throw new InvalidOperationException("Loading should have succeeded");
            }

            var resultingException = InMemoryObject.CreateEmpty("#DatenMeister.Models.ExtentManager.ImportException");
            resultingException.set("message", "Unknown file extension (.xmi and .xml are supported)");
            throw new DatenMeisterException(resultingException);
        }
    }
}