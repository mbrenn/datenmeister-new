namespace DatenMeister.Forms.FormCreator;

/// <summary>
/// This class contains additional properties which can be used for the auto-generation
/// of Collection Forms
/// </summary>
[Obsolete]
public class CollectionFormConfiguration
{
    /// <summary>
    /// A list of extent types which are used to figure out default types being associated
    /// to these extent types. This information is used to create the correct defaultTypes for
    /// the collection form which are then used to create the buttons for the user. 
    /// </summary>
    public List<string> ExtentTypes { get; set; } = new();
}