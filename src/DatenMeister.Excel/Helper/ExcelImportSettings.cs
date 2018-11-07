using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Excel.Helper
{
    public class ExcelImportSettings : ExcelSettings
    {
        /// <summary>
        /// Gets or sets the path of the excel import settings in which the create Xmi-Extent shall be stored
        /// </summary>
        public string extentPath { get; set; }
    }
}