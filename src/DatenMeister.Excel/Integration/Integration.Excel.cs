using DatenMeister.Excel.EMOF;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;

namespace DatenMeister.Excel.Integration
{
    public static class Integration
    {
        public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, ExcelExtentSettings settings = null)
        {
            settings = settings ?? new ExcelExtentSettings();
            var dataProvider = new ExcelDataProvider();
            return dataProvider.LoadProvider(settings);
        }

        public static ExcelProvider LoadExcel(this IDatenMeisterScope container, string url, string filePath)
        {
            var settings = new ExcelExtentSettings
            {
                filePath = filePath
            };

            var dataProvider = new ExcelDataProvider();
            return dataProvider.LoadProvider(settings);
        }
    }
}