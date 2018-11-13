using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.Extents;

using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.Copier
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
            var importSettings = DotNetConverter.ConvertToDotNetObject<ImportSettings>(mofImportSettings);

            if (importSettings.fileToBeImported != importSettings.fileToBeExported)
            {
                File.Copy(importSettings.fileToBeImported, importSettings.fileToBeExported);
            }

            var resultingExtent = _extentManager.LoadExtent(new XmiStorageConfiguration
            {
                extentUri =  importSettings.newExtentUri,
                filePath = importSettings.fileToBeExported,
                workspaceId = importSettings.Workspace
                    }, false);

            return resultingExtent;
        }
    }
}