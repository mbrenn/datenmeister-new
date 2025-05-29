namespace DatenMeister.WebServer.Controller;

/// <summary>
///  Defines the filterparameter being used to query and order the retrieved elements. 
/// </summary>
public class QueryFilterParameter
{
    /// <summary>
    /// Defines the property to which the result shall be ordered
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Defines whether the ordering shall be ascending or descending
    /// </summary>
    public bool OrderByDescending { get; set; }

    /// <summary>
    /// Defines a dictionary of property names to property values to which the filter shall be exected
    /// </summary>
    public Dictionary<string, string> FilterByProperties { get; set; } =
        new();

    /// <summary>
    /// Defines a free text against which the filtering shall be performed
    /// </summary>
    public string? FilterByFreeText { get; set; }
}