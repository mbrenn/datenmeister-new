using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.DependencyInjection;
using DatenMeister.Excel.EMOF;
using DatenMeister.Excel.ProviderLoader;

namespace DatenMeister.Excel.Integration;

public static class Integration
{
    public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, IElement settings)
    {
        return ExcelFileProviderLoader.LoadProvider(settings);
    }

    public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, string filePath)
    {
            
        var settings = InMemoryObject.CreateEmpty(
            _DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelExtentLoaderConfig);
        settings.set(_DatenMeister._ExtentLoaderConfigs._ExcelExtentLoaderConfig.extentUri,
            url);
        settings.set(_DatenMeister._ExtentLoaderConfigs._ExcelExtentLoaderConfig.filePath,
            filePath);
            
        return ExcelFileProviderLoader.LoadProvider(settings);
    }
}