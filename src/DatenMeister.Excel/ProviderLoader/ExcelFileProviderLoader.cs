using System.Diagnostics;
using System.IO;
using DatenMeister.Excel.EMOF;
using DatenMeister.Excel.Helper;
using DatenMeister.Provider;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.ProviderLoader
{
    [ConfiguredBy(typeof(ExcelExtentLoaderConfig))]
    public class ExcelFileProviderLoader : IProviderLoader
    {
        /// <summary>
        /// Loads an excel file and returns
        /// </summary>
        /// <param name="excelPath"></param>
        public static ExcelProvider LoadProvider(ExcelExtentLoaderConfig settings)
        {
            settings = settings ?? new ExcelExtentLoaderConfig("dm:///excel");
            if (!File.Exists(settings.filePath))
            {
                throw new IOException($"File not found: {settings.filePath}");
            }

            var workbook = new XSSFWorkbook(settings.filePath);
            return new ExcelProvider(workbook, settings);
        }

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            var excelFile = (ExcelExtentLoaderConfig) configuration;
            var excelProvider = LoadProvider(excelFile);

            return new LoadedProviderInfo(excelProvider);
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            Debug.Write("Not implemented up to now");
        }
    }
}