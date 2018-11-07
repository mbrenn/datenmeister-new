using System.Diagnostics;
using System.IO;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.EMOF;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Provider;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.ProviderLoader
{
    [ConfiguredBy(typeof(ExcelExtentSettings))]
    public class ExcelFileProviderLoader : IProviderLoader
    {
        /// <summary>
        /// Loads an excel file and returns
        /// </summary>
        /// <param name="excelPath"></param>
        public static ExcelProvider LoadProvider(ExcelExtentSettings settings = null)
        {
            settings = settings ?? new ExcelExtentSettings();
            if (!File.Exists(settings.filePath))
            {
                throw new IOException($"File not found: {settings.filePath}");
            }

            var workbook = new XSSFWorkbook(settings.filePath);
            return new ExcelProvider(workbook, settings);
        }

        private readonly IDatenMeisterScope _scope;

        public ExcelFileProviderLoader(ILifetimeScope scope)
        {
            _scope = (IDatenMeisterScope) scope;
        }

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, bool createAlsoEmpty)
        {
            var excelFile = (ExcelExtentSettings) configuration;
            var excelProvider = LoadProvider(excelFile);

            return new LoadedProviderInfo(excelProvider);
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            Debug.Write("Not implemented up to now");
        }
    }
}