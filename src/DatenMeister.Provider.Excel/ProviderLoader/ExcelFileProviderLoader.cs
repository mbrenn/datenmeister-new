using System.Diagnostics;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Provider.Excel.EMOF;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Provider.Excel.ProviderLoader;

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
            settings.getOrDefault<string>(_ExtentLoaderConfigs._ExcelExtentLoaderConfig.filePath);
            
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
        new()
        {
            IsPersistant = true,
            AreChangesPersistant = false
        };
}