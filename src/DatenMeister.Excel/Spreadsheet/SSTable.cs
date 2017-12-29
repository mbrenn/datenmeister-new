using DatenMeister.Excel.Helper;
using NPOI.SS.UserModel;

namespace DatenMeister.Excel.Spreadsheet
{
    public class SsTable
    {
        private readonly ISheet _sheet;

        public SsTable(ISheet sheet)
        {
            _sheet = sheet;
        }

        public string Name => _sheet.SheetName;

        /// <summary>
        /// Gets the cell content at a specific position in the spreadsheet
        /// </summary>
        /// <param name="row">Row to be queried</param>
        /// <param name="column">Column to be queried</param>
        /// <returns>The found object as object. Null, if not exist</returns>
        public string GetCellContent(int row, int column)
        {
            return _sheet?.GetRow(row)?.GetCell(column)?.GetStringContent();
        }

        public SsCell GetCell(int row, int column)
        {
            var nativeCell = _sheet?.GetRow(row)?.GetCell(column);
            if (nativeCell == null)
            {
                return null;
            }
        
            return new SsCell(nativeCell);
        }
    }
}