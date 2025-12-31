namespace DatenMeister.Provider.Excel.Models;

public class Workbook
{
    public IEnumerable<Table>? tables { get; set; }
}