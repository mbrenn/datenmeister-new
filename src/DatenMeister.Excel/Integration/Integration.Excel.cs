using DatenMeister.Excel.EMOF;
using DatenMeister.Excel.Helper;
using DatenMeister.Excel.ProviderLoader;
using DatenMeister.Integration;

namespace DatenMeister.Excel.Integration
{
    public static class Integration
    {
        public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, ExcelExtentLoaderConfig settings = null)
        {
            settings ??= new ExcelExtentLoaderConfig(url);
            return ExcelFileProviderLoader.LoadProvider(settings);
        }

        public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, string filePath)
        {
            var settings = new ExcelExtentLoaderConfig(url)
            {
                filePath = filePath
            };
            
            return ExcelFileProviderLoader.LoadProvider(settings);
        }
    }
}