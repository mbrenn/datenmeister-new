using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Excel.Helper
{
    /// <summary>
    /// Defines the configuration settings for using the excel sheet as configuration structure for
    /// the extent
    /// </summary>
    public class ExcelExtentSettings : ExtentLoaderConfig
    {
        public string filePath { get; set; }
        public string idColumnName { get; set; }
    }
}