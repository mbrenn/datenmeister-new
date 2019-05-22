using System.Collections.Generic;
using System.IO;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.Spreadsheet
{
    /// <summary>
    /// Gives a native abstraction to direct access to the cells
    /// of the workbook. No conversion or other 'intelligent' data
    /// interpretation is performed
    /// </summary>
    public class SSDocument
    {
        private readonly XSSFWorkbook _document;

        public SSDocument(XSSFWorkbook document)
        {
            _document = document;
        }

        public IEnumerable<SsTable> Tables
        {
            get
            {
                var result = new List<SsTable>();
                lock (_document)
                {
                    var count = _document.NumberOfSheets;

                    for (var n = 0; n < count; n++)
                    {
                        result.Add(new SsTable(_document.GetSheetAt(n)));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Loads an excel data by file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SSDocument LoadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new IOException($"File not found: {path}");
            }
            var document = new XSSFWorkbook(path);
            return new SSDocument(document);
        }
    }
}