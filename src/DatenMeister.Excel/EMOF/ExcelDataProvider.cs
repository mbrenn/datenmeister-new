using System.IO;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.EMOF
{
    public class ExcelDataProvider
    {
        /// <summary>
        /// Loads an excel file and returns
        /// </summary>
        /// <param name="url">Url of the extent to be loeded</param>
        /// <param name="excelPath"></param>
        public ExcelExtent LoadExtent(string url, string excelPath, ExcelSettings settings = null)
        {
            settings = settings ?? new ExcelSettings();
            if (!File.Exists(excelPath))
            {
                throw new IOException($"File not found: {excelPath}");
            }

            var workbook = new XSSFWorkbook(excelPath);
            return new ExcelExtent(url, workbook, settings);

        }
    }
}