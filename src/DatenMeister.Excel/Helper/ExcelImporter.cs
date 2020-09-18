using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Spreadsheet;
using DatenMeister.Models;
using DatenMeister.Runtime;

namespace DatenMeister.Excel.Helper
{
    public class ExcelImporter
    {
        /// <summary>
        /// Gets or sets the settings to be used for the excel importer.
        /// Of Type ExcelLoaderConfig
        /// </summary>
        public IElement LoaderConfig { get; set; }

        private SSDocument? _excelDocument;

        public ExcelImporter(IElement loaderConfig)
        {
            LoaderConfig = loaderConfig ?? throw new ArgumentNullException(nameof(loaderConfig));
        }

        /// <summary>
        /// Loads the excel into the given file path
        /// </summary>
        public void LoadExcel()
        {
            _excelDocument = SSDocument.LoadFromFile(LoaderConfig.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.filePath));
            LoaderConfig.set(
                _DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.sheetName,
                _excelDocument.Tables.FirstOrDefault()?.Name);
        }

        /// <summary>
        /// Gets the names of the sheets
        /// </summary>
        public IEnumerable<string> SheetNames
        {
            get
            {
                return _excelDocument?.Tables.Select(sheets => sheets.Name) ??
                       throw new InvalidOperationException("Excel Document is not loaded");
            }
        }

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private SsTable? GetSheet(string sheet)
        {
            if (!IsExcelLoaded)
            {
                return null;
            }

            return _excelDocument?.Tables.FirstOrDefault(x => x.Name == sheet) ??
                   throw new InvalidOperationException("Excel Document is not loaded");
        }

        private SsTable? GetSelectedSheet()
        {
            if (IsExcelLoaded)
            {
                return GetSheet(LoaderConfig.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.sheetName));
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
            var offsetRow =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
            var offsetColumn =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
            var hasHeader =
                LoaderConfig.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.hasHeader);
            return foundSheet?.GetCellContent(
                row + offsetRow + (hasHeader ? 1 : 0),
                column + offsetColumn) ?? string.Empty;
        }

        /// <summary>
        /// Gets the column names
        /// </summary>
        /// <returns>List of Columns</returns>
        public List<string> GetColumnNames()
        {
            var selectedSheet = GetSelectedSheet();
            
            var offsetRow =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
            var offsetColumn =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
            var hasHeader =
                LoaderConfig.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.hasHeader);

            if (selectedSheet == null) return new List<string>();
            
            // Get Header Rows
            var columnNames = new List<string>();
            var columnCount = GuessColumnCount();
            if (!hasHeader)
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
                    var columnName = selectedSheet.GetCellContent(offsetRow, c + offsetColumn);
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
            var xOffsetRow =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
            var xOffsetColumn =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
            var xHasHeader =
                LoaderConfig.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.hasHeader);
            var xCountRows =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.countRows);
            var xFitRowCount =
                LoaderConfig.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.fixRowCount);
            
            var hasHeaderRows = xHasHeader;
            var foundSheet = GetSelectedSheet();
            if (foundSheet == null) return -1;
            if (xFitRowCount) return xCountRows;

            var offsetRow = hasHeaderRows ? xOffsetRow + 1 : xOffsetRow;
            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(offsetRow + n, xOffsetColumn);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            LoaderConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.countRows, n);
            return n;
        }

        /// <summary>
        /// Guesses the number of columns
        /// </summary>
        public int GuessColumnCount()
        {
            var foundSheet = GetSelectedSheet();
            if (foundSheet == null) return -1;
            
            var xOffsetRow =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
            var xOffsetColumn =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
            var xCountColumns =
                LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.countColumns);
            var xFitRowCount =
                LoaderConfig.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.fixRowCount);

            if (xFitRowCount) return xCountColumns;


            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(xOffsetRow, xOffsetColumn + n);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            LoaderConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelLoaderConfig.countColumns, n);
            return n;
        }
    }
}