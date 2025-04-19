namespace DatenMeister.Provider.Json;

public class JsonCompressedImporterSettings
{
    /// <summary>
    /// Defines the name in which the compressed data is given
    /// </summary>
    public string ColumnPropertyName { get; set; } = "columns";
    
    /// <summary>
    /// Defines the name in which the compressed data is given
    /// </summary>
    public string DataPropertyName { get; set; } = "data";
    
    public Func<string, bool> FilterProperty { get; set; } = _ => true;
}