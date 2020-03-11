using System;
using System.IO;
using DatenMeister.Core.EMOF.Implementation;
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

        public IUriExtent ImportExtent(IObject mofImportSettings)
        {
            var importSettings = DotNetConverter.ConvertToDotNetObject<ImportSettings>(mofImportSettings)
                                 ?? throw new InvalidOperationException("mofImportSettings == null");
            var extentUri = importSettings.newExtentUri;
            if (extentUri == null)
                throw new InvalidOperationException("extentUri == null");

            if (importSettings.fileToBeImported != importSettings.fileToBeExported)
            {
                File.Copy(importSettings.fileToBeImported, importSettings.fileToBeExported);
            }

            var resultingExtent = _extentManager.LoadExtent(new XmiStorageConfiguration(extentUri)
            {
                filePath = importSettings.fileToBeExported,
                workspaceId = importSettings.Workspace
            });
            
            if ( resultingExtent == null )
                 throw new InvalidOperationException("Loading did not succeed");

            return resultingExtent;
        }
    }
}