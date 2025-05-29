namespace DatenMeister.Excel.Models;

public static class ExcelModels
{
    public static IEnumerable<Type> AllTypes =>
    [
        typeof(Workbook),
        typeof(Table)
    ];
}