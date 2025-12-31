using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.DependencyInjection;
using DatenMeister.Provider.Excel.EMOF;
using DatenMeister.Provider.Excel.ProviderLoader;

namespace DatenMeister.Provider.Excel.Integration;

public static class Integration
{
    public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, IElement settings)
    {
        return ExcelFileProviderLoader.LoadProvider(settings);
    }

    public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, string filePath)
    {
            
        var settings = InMemoryObject.CreateEmpty(
            _ExtentLoaderConfigs.TheOne.__ExcelExtentLoaderConfig);
        settings.set(_ExtentLoaderConfigs._ExcelExtentLoaderConfig.extentUri,
            url);
        settings.set(_ExtentLoaderConfigs._ExcelExtentLoaderConfig.filePath,
            filePath);
            
        return ExcelFileProviderLoader.LoadProvider(settings);
    }
}