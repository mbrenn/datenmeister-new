using System.Text.Json.Nodes;
using DatenMeister.Core.EMOF.Interface.Reflection;

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

    /// <summary>
    /// Allows a renaming of the properties.
    /// Goal is to rename the properties so they match to FieldNamesEwm.
    /// If null is returned, the field property is being ignored fully. 
    /// </summary>
    public Func<string, string?> RenameProperty { get; set; } = x => x;
    
    /// <summary>
    /// Used to filter-out the properties.
    /// In case the method returns false, the property is not evaluated
    /// </summary>
    public Func<string, bool> FilterProperty { get; set; } = _ => true;
    
    /// <summary>
    /// Converts the properties for a certain string
    /// </summary>
    public Func<string, object?, object?> ConvertProperty { get; set; } = (_, value) => value;

    public Predicate<IElement> FilterItemOut { get; set; } = (_) => false;

    public Func<Dictionary<string, int>, JsonArray, bool> FilterJsonOut { get; set; } = (_, _) => false;
}