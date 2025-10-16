using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Extent.Manager.Extents.Configuration;

/// <summary>
/// Stores the setting of the extent
/// </summary>
public class ExtentType(string name)
{
    /// <summary>
    /// Gets or sets the name of the extent type to which the extent type is added
    /// </summary>
    public string name { get; set; } = name;

    /// <summary>
    /// Stores the metaclasses of the root elements which are preferred to get added
    /// to the extent. Other metaclasses are also possible, but these one are actively offered
    /// by the tool.
    /// </summary>
    public List<IElement> rootElementMetaClasses { get; } = new();

    public override string ToString()
    {
        return name ?? "{ExtentType}";
    }
}