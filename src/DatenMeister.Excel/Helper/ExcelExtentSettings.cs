namespace DatenMeister.Excel.Helper
{
    /// <summary>
    /// Defines the configuration settings for using the excel sheet as configuration structure for
    /// the extent
    /// </summary>
    public class ExcelExtentSettings
    {
        public string filePath { get; set; }
        public string idColumnName { get; set; }

        public string workspaceId { get; set; }
        public string extentUri { get; set; }
        public string extentPath { get; set; }
    }
}