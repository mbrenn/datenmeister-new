namespace DatenMeister.Excel.Models;

public class Workbook
{
    public IEnumerable<Table>? tables { get; set; }
}