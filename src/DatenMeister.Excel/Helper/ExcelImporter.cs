using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Excel.Properties;
using DatenMeister.Excel.Spreadsheet;

namespace DatenMeister.Excel.Helper
{
    public class ExcelImporter
    {
        /// <summary>
        /// Gets or sets the settings to be used for the excel importer
        /// </summary>
        public ExcelLoaderConfig LoaderConfig { get; set; }

        private SSDocument _excelDocument;

        public ExcelImporter([NotNull] ExcelLoaderConfig loaderConfig)
        {
            LoaderConfig = loaderConfig ?? throw new ArgumentNullException(nameof(loaderConfig));
        }

        /// <summary>
        /// Loads the excel into the given file path
        /// </summary>
        public void LoadExcel()
        {
            _excelDocument = SSDocument.LoadFromFile(LoaderConfig.filePath);
            LoaderConfig.sheetName = _excelDocument.Tables.FirstOrDefault()?.Name;
        }

        /// <summary>
        /// Gets the names of the sheets
        /// </summary>
        public IEnumerable<string> SheetNames
        {
            get { return _excelDocument.Tables.Select(sheets => sheets.Name); }
        }

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private SsTable? GetSheet(string sheet)
        {
            if (sheet == null || !IsExcelLoaded)
            {
                return null;
            }

            return _excelDocument.Tables.FirstOrDefault(x => x.Name == sheet);
        }

        private SsTable? GetSelectedSheet()
        {
            if (IsExcelLoaded)
            {
                return GetSheet(LoaderConfig.sheetName);
            }

            return null;
        }

        /// <summary>
        /// Gets or sets the flag whether the excel file is loaded
        /// </summary>
        public bool IsExcelLoaded => _excelDocument != null;

        /// <summary>
        /// Gets the cellcontent of a certain row and column.
        /// The importer converts the range of 0 to row/column to the exact position within the excel file.
        /// Consumers of the excel don't need to handle offset and other things.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetCellContent(int row, int column)
        {
            var foundSheet = GetSelectedSheet();
            return foundSheet?.GetCellContent(
                row + LoaderConfig.offsetRow + (LoaderConfig.hasHeader ? 1 : 0),
                column + LoaderConfig.offsetColumn);
        }

        /// <summary>
        /// Gets the column names
        /// </summary>
        /// <returns>List of Columns</returns>
        public List<string> GetColumnNames()
        {
            var selectedSheet = GetSelectedSheet();

            if (selectedSheet == null) return null;
            // Get Header Rows
            var columnNames = new List<string>();
            var columnCount = GuessColumnCount();
            if (!LoaderConfig.hasHeader)
            {
                for (var c = 0; c < columnCount; c++)
                {
                    columnNames.Add("_" + c);
                }
            }
            else
            {
                for (var c = 0; c < columnCount; c++)
                {
                    var columnName = selectedSheet.GetCellContent(LoaderConfig.offsetRow, c + LoaderConfig.offsetColumn);
                    if (string.IsNullOrEmpty(columnName) || columnNames.Contains(columnName))
                    {
                        columnNames.Add(null);
                    }
                    else
                    {
                        columnNames.Add(columnName);
                    }
                }
            }
            return columnNames;
        }

        public int GuessRowCount()
        {
            var hasHeaderRows = LoaderConfig.hasHeader;
            var foundSheet = GetSelectedSheet();
            if (foundSheet == null) return -1;
            if (LoaderConfig.fixRowCount) return LoaderConfig.countRows;

            var offsetRow = hasHeaderRows ? LoaderConfig.offsetRow + 1 : LoaderConfig.offsetRow;
            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(offsetRow + n, LoaderConfig.offsetColumn);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            LoaderConfig.countRows = n;
            return n;
        }

        /// <summary>
        /// Guesses the number of columns
        /// </summary>
        public int GuessColumnCount()
        {
            var foundSheet = GetSelectedSheet();
            if (foundSheet == null) return -1;

            if (LoaderConfig.fixColumnCount) return LoaderConfig.countColumns;


            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(LoaderConfig.offsetRow, LoaderConfig.offsetColumn + n);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            LoaderConfig.countColumns = n;
            return n;
        }
    }
}