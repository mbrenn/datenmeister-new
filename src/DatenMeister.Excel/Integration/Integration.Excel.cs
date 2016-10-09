using DatenMeister.Excel.EMOF;
using DatenMeister.Integration;

namespace DatenMeister.Excel.Integration
{
    public static class Integration
    {
        public static ExcelExtent LoadExcel(this IDatenMeisterScope container, string url, string path, ExcelSettings settings = null)
        {
            settings = settings ?? new ExcelSettings();
            var dataProvider = new ExcelDataProvider();
            return dataProvider.LoadExtent(url, path, settings);
        }
    }
}