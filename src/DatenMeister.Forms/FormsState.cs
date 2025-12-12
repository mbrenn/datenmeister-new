namespace DatenMeister.Forms;

/// <summary>
/// Defines the data-structure for the form modification plugin
/// </summary>
public class FormModificationPlugin
{
    /// <summary>
    /// Function which creates the context
    /// </summary>
    public required Action<FormCreationContext> CreateContext { get; set; }
    
    /// <summary>
    /// Name of the plugin
    /// </summary>
    public required string Name { get; set; }
}


public class FormsState
{
    /// <summary>
    /// Stores the list of form modification plugins which may modify the factory 
    /// </summary>
    public List<FormModificationPlugin> FormModificationPlugins { get; } = [];
}