namespace DatenMeister.Core.TypeIndexAssembly;

/// <summary>
/// This class stores the permanent information for the TypeIndexStorage.
/// It contains the data itself, but in addition also synchronization mechanisms
/// so the type index can be updated offline
/// </summary>
public class TypeIndexStore
{
    /// <summary>
    /// Gets or sets the currentl data for the TypeIndexing
    /// </summary>
    public TypeIndexData? Current { get; set; }
    
    /// <summary>
    /// Defines the monitor for the swapping if the index data
    /// </summary>
    public object IndexSwapping { get; } = new object();
    
    /// <summary>
    /// Defines the monitor for the building of the index.
    /// This monitor is held to be absolutely sure that only one indexing is happening at the same time. 
    /// </summary>
    public object IndexBuild { get; } = new object();
    
    
}