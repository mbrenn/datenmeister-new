using NPOI.SS.UserModel;

namespace DatenMeister.Excel.Spreadsheet;

public class SsCell
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ICell _nativeCell;

    public SsCell(ICell nativeCell)
    {
        _nativeCell = nativeCell;
    }
}