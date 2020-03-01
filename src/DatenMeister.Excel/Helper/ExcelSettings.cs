using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Excel.Helper
{
    public class ExcelSettings : ExtentLoaderConfig
    {
        public bool fixRowCount { get; set; }
        public bool fixColumnCount { get; set; }
        public string? filePath { get; set; }
        public string? sheetName { get; set; }
        public int offsetRow { get; set; }
        public int offsetColumn { get; set; }
        public int countRows { get; set; }
        public int countColumns { get; set; }
        public bool hasHeader { get; set; } = true;
        public string? idColumnName { get; set; }

        /// <summary>
        /// Gets the settings as a mof object
        /// </summary>
        /// <returns></returns>
        public IObject GetSettingsAsMofObject()
        {
            return DotNetConverter.ConvertFromDotNetObject(this);
        }

        public ExcelSettings()
        {
            
        }

        public ExcelSettings(string extentUri) : base(extentUri)
        {
        }
    }
}