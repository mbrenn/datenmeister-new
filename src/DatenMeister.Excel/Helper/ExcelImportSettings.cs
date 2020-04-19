namespace DatenMeister.Excel.Helper
{
    public class ExcelImportSettings : ExcelSettings
    {
        /// <summary>
        /// Gets or sets the path of the excel import settings in which the create Xmi-Extent shall be stored
        /// </summary>
        public string? extentPath { get; set; }

        public ExcelImportSettings()
        {
            
        }
        
        public ExcelImportSettings(string extentUri) : base(extentUri)
        {
        }
    }
}