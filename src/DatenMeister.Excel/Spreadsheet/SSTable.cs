using DatenMeister.Excel.Helper;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace DatenMeister.Excel.Spreadsheet;

public class SsTable(ISheet sheet)
{
    public string Name => sheet.SheetName;

    /// <summary>
    /// Gets the cell content at a specific position in the spreadsheet
    /// </summary>
    /// <param name="row">Row to be queried</param>
    /// <param name="column">Column to be queried</param>
    /// <returns>The found object as object. Null, if not exist</returns>
    public string GetCellContent(int row, int column)
    {
        var cell = GetFirstCellInMergedRegionContainingCell(
            sheet?.GetRow(row)?.GetCell(column));
        return cell?.GetStringContent() ?? string.Empty;
    }
        
    /// <summary>
    /// Gets the merged cell, if merged
    /// https://stackoverflow.com/questions/61013362/using-npoi-to-retrieve-the-value-of-a-merged-cell-from-an-excel-spreadsheet
    /// </summary>
    /// <param name="cell">Cell to be checked</param>
    /// <returns>The merged cell or the cell itself</returns>
    private static ICell? GetFirstCellInMergedRegionContainingCell(ICell? cell)
    {
        if (cell != null && cell.IsMergedCell)
        {
            ISheet sheet = cell.Sheet;
            for (int i = 0; i < sheet.NumMergedRegions; i++)
            {
                CellRangeAddress region = sheet.GetMergedRegion(i);
                if (region.ContainsRow(cell.RowIndex) && 
                    region.ContainsColumn(cell.ColumnIndex))
                {
                    IRow row = sheet.GetRow(region.FirstRow);
                    ICell? firstCell = row?.GetCell(region.FirstColumn);
                    return firstCell;
                }
            }
            return null;
        }
        return cell;
    }

    public SsCell? GetCell(int row, int column)
    {
        var nativeCell = sheet?.GetRow(row)?.GetCell(column);
        if (nativeCell == null)
        {
            return null;
        }
        
        return new SsCell(nativeCell);
    }
}