using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Excel.Spreadsheet;

namespace DatenMeister.Excel.Helper;

public class ExcelImporter
{
    public class Column
    {
        /// <summary>
        /// Initializes a new instance of the Column
        /// </summary>
        /// <param name="name">Name of the column</param>
        /// <param name="header">Header of the column</param>
        public Column(string name, string header)
        {
            Name = name;
            Header = header;
        }
            
        /// <summary>
        /// Name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the header
        /// </summary>
        public string Header { get; set; }
    }
        
    /// <summary>
    /// Gets or sets the settings to be used for the excel importer.
    /// Of Type ExcelLoaderConfig
    /// </summary>
    public IElement LoaderConfig { get; set; }
        
    private readonly ExcelColumnTranslator _columnTranslator = new();
        
    /// <summary>
    /// Gets the column translator being used to figure out internal column name
    /// </summary>
    public ExcelColumnTranslator ColumnTranslator => _columnTranslator;

    private SSDocument? _excelDocument;

    public ExcelImporter(IElement loaderConfig)
    {
        LoaderConfig = loaderConfig ?? throw new ArgumentNullException(nameof(loaderConfig));
        _columnTranslator.LoadTranslation(loaderConfig);
    }

    /// <summary>
    /// Loads the excel into the given file path
    /// </summary>
    public void LoadExcel()
    {
        _excelDocument = SSDocument.LoadFromFile(LoaderConfig.getOrDefault<string>(_ExtentLoaderConfigs._ExcelLoaderConfig.filePath));

        if (!LoaderConfig.isSet(_ExtentLoaderConfigs._ExcelLoaderConfig.sheetName))
        {
            LoaderConfig.set(
                _ExtentLoaderConfigs._ExcelLoaderConfig.sheetName,
                _excelDocument.Tables.FirstOrDefault()?.Name);
        }
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

    private SsTable? _cachedSelectedSheet;
    private SsTable? GetSelectedSheet()
    {
        if (_cachedSelectedSheet != null)
        {
            return _cachedSelectedSheet;
        }
        
        if (IsExcelLoaded)
        {
            return _cachedSelectedSheet = 
                GetSheet(LoaderConfig.getOrDefault<string>(_ExtentLoaderConfigs._ExcelLoaderConfig.sheetName));
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
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
        var offsetColumn =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
        var hasHeader =
            LoaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelLoaderConfig.hasHeader);
        return foundSheet?.GetCellContent(
            row + offsetRow + (hasHeader ? 1 : 0),
            column + offsetColumn) ?? string.Empty;
    }

    /// <summary>
    /// Gets the column names after the transformation has been done
    /// </summary>
    /// <returns>List of Columns</returns>
    public IEnumerable<string> GetOriginalColumnNames()
    {
        var selectedSheet = GetSelectedSheet();
            
        var offsetRow =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
        var offsetColumn =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
        var hasHeader =
            LoaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelLoaderConfig.hasHeader);
        var tryMergedHeaderCells =
            LoaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelLoaderConfig.tryMergedHeaderCells);

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
                    
                if (tryMergedHeaderCells)
                {
                    // If header rows might be merged with the cell above, it is tried
                    // to get the cell value above the currently selected header cell
                    var row = offsetRow;
                    while (string.IsNullOrEmpty(columnName) && row > 0)
                    {
                        row--;
                        columnName = selectedSheet.GetCellContent(row, c + offsetColumn);
                    }
                }
                    
                if (string.IsNullOrEmpty(columnName) || columnNames.Contains(columnName))
                {
                    columnNames.Add(string.Empty);
                }
                else
                {
                    columnNames.Add(columnName);
                }
            }
        }
            
        return columnNames;
    }

    /// <summary>
    /// Gets the column names including the translation 
    /// </summary>
    /// <param name="forceAll">Flag, whether all parameter shall be forced to be returned</param>
    /// <returns>Enumeration of column names</returns>
    public IEnumerable<string> GetColumnNames(bool forceAll = false)
    {
        var onlySetColumn =
            LoaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelLoaderConfig.onlySetColumns);
            
        var list = GetOriginalColumnNames();
        foreach (var headerName in list)
        {
            if (onlySetColumn && !forceAll)
            {
                var result = _columnTranslator.TranslateHeaderOrNull(headerName);
                if (result != null)
                {
                    yield return result;
                }
            }
            else
            {
                yield return _columnTranslator.TranslateHeader(headerName);    
            }
        }
    }

    public int GuessRowCount()
    {
        var xOffsetRow =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
        var xOffsetColumn =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
        var xHasHeader =
            LoaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelLoaderConfig.hasHeader);
        var xCountRows =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.countRows);
        var xFitRowCount =
            LoaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelLoaderConfig.fixRowCount);
        var xSkipEmptyRows =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.skipEmptyRowsCount);
            
        var hasHeaderRows = xHasHeader;
        var foundSheet = GetSelectedSheet();
        if (foundSheet == null) return -1;
        if (xFitRowCount) return xCountRows;

        var offsetRow = hasHeaderRows ? xOffsetRow + 1 : xOffsetRow;
        var n = 0;
        var skippedRows = 0;
        while (true)
        {
            var content = foundSheet.GetCellContent(offsetRow + n, xOffsetColumn);
            if (string.IsNullOrEmpty(content))
            {
                skippedRows++;
            }
            else
            {
                skippedRows = 0;
            }

            if (skippedRows > xSkipEmptyRows)
            {
                break;
            }

            n++;
        }

        LoaderConfig.set(_ExtentLoaderConfigs._ExcelLoaderConfig.countRows, n);
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
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetRow);
        var xOffsetColumn =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.offsetColumn);
        var xCountColumns =
            LoaderConfig.getOrDefault<int>(_ExtentLoaderConfigs._ExcelLoaderConfig.countColumns);
        var xFitRowCount =
            LoaderConfig.getOrDefault<bool>(_ExtentLoaderConfigs._ExcelLoaderConfig.fixRowCount);

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

        LoaderConfig.set(_ExtentLoaderConfigs._ExcelLoaderConfig.countColumns, n);
        return n;
    }
}