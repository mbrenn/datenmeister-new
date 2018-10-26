using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Annotations;
using DatenMeister.Excel.Spreadsheet;
using DatenMeister.Integration;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Excel.Helper
{
    public class ExcelImporter
    {
        /// <summary>
        /// Gets or sets the settings to be used for the excel importer
        /// </summary>
        public ExcelImportSettings Settings { get; set; }

        private SSDocument _excelDocument;

        public ExcelImporter([NotNull] ExcelImportSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Loads the excel into the given file path
        /// </summary>
        /// <param name="filePath">The excel file to be loaded</param>
        public void LoadExcel()
        {
            _excelDocument = SSDocument.LoadFromFile(Settings.filePath);
            Settings.sheetName = _excelDocument.Tables.FirstOrDefault()?.Name;
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
        private SsTable GetSheet(string sheet)
        {
            if (sheet == null || !IsExcelLoaded)
            {
                return null;
            }

            return _excelDocument.Tables.FirstOrDefault(x => x.Name == sheet);
        }

        private SsTable GetSelectedSheet()
        {
            if (IsExcelLoaded)
            {
                return GetSheet(Settings.sheetName);
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
                row + Settings.offsetRow + (Settings.hasHeader ? 1 : 0),
                column + Settings.offsetColumn);
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
            if (!Settings.hasHeader)
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
                    var columnName = selectedSheet.GetCellContent(Settings.offsetRow, c + Settings.offsetColumn);
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
            var hasHeaderRows = Settings.hasHeader;
            var foundSheet = GetSelectedSheet();
            if (foundSheet == null) return -1;
            if (Settings.fixRowCount) return Settings.countRows;

            var offsetRow = hasHeaderRows ? Settings.offsetRow + 1 : Settings.offsetRow;
            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(offsetRow + n, Settings.offsetColumn);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            Settings.countRows = n;
            return n;
        }

        /// <summary>
        /// Guesses the number of columns
        /// </summary>
        public int GuessColumnCount()
        {

            var foundSheet = GetSelectedSheet();
            if (foundSheet == null) return -1;

            if (Settings.fixColumnCount) return Settings.countColumns;


            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(Settings.offsetRow, Settings.offsetColumn + n);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            Settings.countColumns = n;
            return n;
        }

        /// <summary>
        /// Performs the import of the excel into the extent
        /// </summary>
        /// <param name="scope">The datenmeister scope to be used</param>
        /// <param name="importSettings">Settings to be used to be imported</param>
        /// <returns></returns>
        public static IUriExtent ImportExcelAsCopy(IDatenMeisterScope scope, IObject importSettings)
        {
            var settings = DotNetConverter.ConvertToDotNetObject<ExcelImportSettings>(importSettings);

            var configuration = new XmiStorageConfiguration
            {
                ExtentUri = settings.extentUri,
                Path = settings.extentPath,
                Workspace = settings.workspaceId
            };

            var excelImporter = new ExcelImporter(settings);

            excelImporter.LoadExcel();

            // Gets the columns names

            var extentManager = scope.Resolve<IExtentManager>();
            var extent = extentManager.LoadExtent(configuration, true);

            var factory = new MofFactory(extent);

            var columnNames = excelImporter.GetColumnNames();
            for (var r = 0; r < settings.countRows; r++)
            {
                var item = factory.create(null);
                for (var c = 0; c < settings.countColumns; c++)
                {
                    var columnName = columnNames[c];
                    if (columnName == null)
                    {
                        // Skip not set columns
                        continue;
                    }

                    item.set(columnName, excelImporter.GetCellContent(r, c));
                }

                extent.elements().add(item);
            }

            return extent;
        }

        public static void ImportExcelAsReference(IDatenMeisterScope scope, IObject getConfigurationObject)
        {
            throw new NotImplementedException();
        }
    }
}