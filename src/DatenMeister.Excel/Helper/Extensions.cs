using System.Globalization;
using NPOI.SS.UserModel;

namespace DatenMeister.Excel.Helper;

public static class Extensions
{

    public static string GetStringContent(this ICell cell)
    {
        var cellType = cell.CellType;
        return GetStringContent(cell, cellType);
    }

    private static string GetStringContent(ICell cell, CellType cellType)
    {
        if (cellType == CellType.Numeric)
        {
            return cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
        }

        if (cellType == CellType.Formula)
        {
            return GetStringContent(cell, cell.CachedFormulaResultType);
        }

        return cell.StringCellValue;
    }
}