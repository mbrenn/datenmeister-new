using NPOI.XSSF.UserModel;

namespace DatenMeister.Provider.Excel.Spreadsheet;

/// <summary>
/// Gives a native abstraction to direct access to the cells
/// of the workbook. No conversion or other 'intelligent' data
/// interpretation is performed
/// </summary>
public class SSDocument(XSSFWorkbook document)
{
    public IEnumerable<SsTable> Tables
    {
        get
        {
            var result = new List<SsTable>();
            lock (document)
            {
                var count = document.NumberOfSheets;

                for (var n = 0; n < count; n++)
                {
                    result.Add(new SsTable(document.GetSheetAt(n)));
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