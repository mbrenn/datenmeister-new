namespace DatenMeister.Provider.Excel.Models;

public class Table
{
    public string? name { get; set; }
    
    public IEnumerable<object>? items { get; set; }
}