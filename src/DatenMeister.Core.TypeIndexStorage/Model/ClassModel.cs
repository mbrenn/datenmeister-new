using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.TypeIndexAssembly.Model;

/// <summary>
/// Stores the properties of the class being evaluated 
/// </summary>
public class ClassModel
{
    /// <summary>
    /// Stores the id of the class
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Stores the full name of the class
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Stores the name of the class
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Stores the uri of the class
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URI of the extent to which the class belongs
    /// </summary>
    public string ExtentUri { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the workspace to which the class belongs
    /// </summary>
    public string WorkspaceId { get; set; } = string.Empty;

    /// <summary>
    /// Stores the Uri of all Generalizations of that class
    /// </summary>
    public List<string> Generalizations { get; } = new();

    /// <summary>
    /// Stores the enumeration of the attributes
    /// </summary>
    public List<AttributeModel> Attributes { get; } = new();

    /// <summary>
    /// Stores the index of the attributes for faster access
    /// </summary>
    private Dictionary<string, AttributeModel>? _attributesIndex;
    
    /// <summary>
    /// Stores the cached element of the class
    /// </summary>
    public IElement? CachedElement { get; set; }

    /// <summary>
    /// Creates the index for the attributes
    /// </summary>
    public void CreateIndex()
    {
        _attributesIndex = Attributes.ToDictionary(x => x.Name, x => x);
    }

    /// <summary>
    /// Gets the attribute by the given name
    /// </summary>
    /// <param name="attribute">Attribute to be queried</param>
    /// <returns>The found attribute or null in case not found</returns>
    public AttributeModel? FindAttribute(string attribute)
    {
        if (_attributesIndex != null)
        {
            return _attributesIndex.GetValueOrDefault(attribute);
        }

        return Attributes.FirstOrDefault(x => x.Name == attribute);
    }
}