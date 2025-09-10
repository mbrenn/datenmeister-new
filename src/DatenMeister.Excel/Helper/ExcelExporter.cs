using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DatenMeister.Excel.Helper;

public class ExcelExporter
{
    public static async Task ExportToExcel(string targetPath, IEnumerable<IElement> elements)
    {
        var properties = new HashSet<string>();

        // Gets the property by reading the first 5 items
        var elementsAsArray = elements as IElement[] ?? elements.ToArray();
        foreach (var element in elementsAsArray.OfType<IObjectAllProperties>())
        {
            foreach (var property in element.getPropertiesBeingSet())
            {
                properties.Add(property);
            }
        }

        var sortedProperties = properties.OrderBy(x => x).ToList();

        // We got the elements, now create the excel.
        var directory = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("Sheet1");

        var n = 0;
        var firstRow = sheet.CreateRow(n);
        var cell = 0;
        foreach (var property in sortedProperties)
        {
            var firstCell = firstRow.CreateCell(cell, CellType.String);
            firstCell.SetCellValue(property);
            cell++;
        }

        sheet.AutoSizeColumn(n);

        foreach (var element in elementsAsArray)
        {
            n++;

            if (n % 100 == 0)
            {
                Console.Write($"** {n}/{elementsAsArray.Length} in storage\r");
            }

            firstRow = sheet.CreateRow(n);
            cell = 0;
            foreach (var property in sortedProperties)
            {
                var firstCell = firstRow.CreateCell(cell++, CellType.String);
                firstCell.SetCellValue(
                    element.getOrDefault<string>(property)?.ShortenString(4096)
                    ?? string.Empty);
            }
        }

        // Autosize the first column for better visibility
        Console.WriteLine();
        Console.WriteLine("** Writing Excel File");

        await Task.Run(() =>
        {
            using var fs = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None);
            workbook.Write(fs);
        });
    }
}