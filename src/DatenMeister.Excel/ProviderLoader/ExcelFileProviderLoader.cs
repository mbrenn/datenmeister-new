using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Excel.EMOF;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.ProviderLoader
{
    public class ExcelFileProviderLoader : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        
        public IScopeStorage? ScopeStorage { get; set; }
        
        /// <summary>
        /// Loads an excel file and returns
        /// </summary>
        /// <param name="settings">The settings being used to load the excel</param>
        public static ExcelProvider LoadProvider(IElement settings)
        {
            var filePath =
                settings.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExcelExtentLoaderConfig.filePath);
            
            if (!File.Exists(filePath))
            {
                throw new IOException($"File not found: {filePath}");
            }

            var workbook = new XSSFWorkbook(filePath);
            
            return new ExcelProvider(workbook, settings);
        }

        public Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var excelProvider = LoadProvider(configuration);

            return Task.FromResult(new LoadedProviderInfo(excelProvider));
        }

        public Task StoreProvider(IProvider extent, IElement configuration)
        {
            Debug.Write("Not implemented up to now");

            return Task.CompletedTask;
        }

        public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } =
            new ProviderLoaderCapabilities
            {
                IsPersistant = true,
                AreChangesPersistant = false
            };
    }
}