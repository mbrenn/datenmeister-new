namespace DatenMeister.Core.TypeIndexAssembly.Model;

/// <summary>
/// Stores the properties of the class being evaluated 
/// </summary>
public record ClassModel
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
    /// Stores the Uri of all Generalizations of that class
    /// </summary>
    public List<string> Generalizations { get; } = new();

    /// <summary>
    /// Stores the enumeration of the attributes
    /// </summary>
    public List<AttributeModel> Attributes { get; } = new();

    /// <summary>
    /// Gets the attribute by the given name
    /// </summary>
    /// <param name="attribute">Attribute to be queried</param>
    /// <returns>The found attribute or null in case not found</returns>
    public AttributeModel? FindAttribute(string attribute)
    {
        return Attributes.FirstOrDefault(x => x.Name == attribute);
    }
}