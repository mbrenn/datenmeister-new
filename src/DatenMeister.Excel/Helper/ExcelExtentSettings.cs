using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Excel.Helper
{
    /// <summary>
    /// Defines the configuration settings for using the excel sheet as configuration structure for
    /// the extent
    /// </summary>
    public class ExcelExtentLoaderConfig : ExtentLoaderConfig
    {
        public ExcelExtentLoaderConfig()
        {
        }
        
        public string? filePath { get; set; }
        public string? idColumnName { get; set; }

        public ExcelExtentLoaderConfig(string extentUri) : base(extentUri)
        {
        }
    }
}