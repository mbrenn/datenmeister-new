using System.IO;
using DatenMeister.Excel.Helper;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class ExcelDataProvider
    {
        /// <summary>
        /// Loads an excel file and returns
        /// </summary>
        /// <param name="excelPath"></param>
        public ExcelProvider LoadProvider(ExcelExtentSettings settings = null)
        {
            settings = settings ?? new ExcelExtentSettings();
            if (!File.Exists(settings.filePath))
            {
                throw new IOException($"File not found: {settings.filePath}");
            }

            var workbook = new XSSFWorkbook(settings.filePath);
            return new ExcelProvider(workbook, settings);
        }
    }
}