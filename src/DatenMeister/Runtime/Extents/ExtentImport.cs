using System;
using System.IO;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.Extents
{
    public class ExtentImport
    {
        private readonly IExtentManager _extentManager;

        public ExtentImport(IExtentManager extentManager )
        {
            _extentManager = extentManager;
        }

        /// <summary>
        /// Imports the file for according the given settings.
        /// The settings are of type "DatenMeister.FormSupport.LoadFileFromXmi"
        /// </summary>
        /// <param name="mofImportSettings">Import settings being used</param>
        /// <returns>The created uri extent</returns>
        public IUriExtent ImportExtent(IObject mofImportSettings)
        {
            var extentUri = mofImportSettings.getOrDefault<string>("extentUri");
            var workspaceId = mofImportSettings.getOrDefault<string>("workspace");
            var filePath = mofImportSettings.getOrDefault<string>("filePath");
            
            var resultingExtent = _extentManager.LoadExtent(new XmiStorageLoaderConfig(extentUri)
            {
                filePath = filePath,
                workspaceId = workspaceId
            });
            
            if ( resultingExtent == null )
                 throw new InvalidOperationException("Loading did not succeed");

            return resultingExtent;
        }
    }
}