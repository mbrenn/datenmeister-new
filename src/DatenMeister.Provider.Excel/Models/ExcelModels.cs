namespace DatenMeister.Provider.Excel.Models;

public static class ExcelModelInfo
{
    public static IEnumerable<Type> AllTypes =>
    [
        typeof(Workbook),
        typeof(Table)
    ];
}