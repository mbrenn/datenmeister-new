using NPOI.SS.UserModel;

namespace DatenMeister.Excel.Spreadsheet;

public class SsCell(ICell nativeCell)
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ICell _nativeCell = nativeCell;
}