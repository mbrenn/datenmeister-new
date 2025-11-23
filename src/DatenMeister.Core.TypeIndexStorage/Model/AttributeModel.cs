namespace DatenMeister.Core.TypeIndexAssembly.Model;

/// <summary>
/// Stores the information about the attribute within the class
/// </summary>
public record AttributeModel
{
    /// <summary>
    /// Gets or sets the Id of the attribute as given within the extent
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the name of the attribute
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Stores the url of the attribute.
    /// </summary>
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the information whether the attribute is a composite attribute
    /// </summary>
    public bool IsComposite { get; set; }
    
    /// <summary>
    /// Stores the url to the type of which this attribute belongs to. 
    /// </summary>
    public string? TypeUrl { get; set; } 
    
    /// <summary>
    /// Defines the default value of the attribute.
    /// </summary>
    public object? DefaultValue { get; set; }
    
    /// <summary>
    /// Gets or sets the flag whether this attribute is a single instance or a multiple instance.
    /// Multiplier >1 or *
    /// </summary>
    public bool IsMultiple { get; set; }
    
    /// <summary>
    /// Gets or sets the flag whether the attribute is inherited from a parent class
    /// </summary>
    public bool IsInherited { get; set; }


}