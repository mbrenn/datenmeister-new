namespace DatenMeister.Core.TypeIndexAssembly;

/// <summary>
/// This class stores the permanent information for the TypeIndexStorage.
/// It contains the data itself, but in addition also synchronization mechanisms
/// so the type index can be updated offline
/// </summary>
public class TypeIndexStore
{
    /// <summary>
    /// Gets or sets the current data for the TypeIndexing
    /// </summary>
    internal TypeIndexData? Current { get; set; }
    
    /// <summary>
    /// Gets or sets the next data for the TypeIndexing which will be swapped to the current data
    /// </summary>
    public TypeIndexData? Next { get; set; }
    
    /// <summary>
    /// Defines the monitor for the swapping if the index data
    /// </summary>
    public object SyncIndexSwapping { get; } = new();
    
    /// <summary>
    /// This event is set, when the index is finished to be built. 
    /// </summary>
    public EventWaitHandle IndexBuiltEvent { get; } = new(false, EventResetMode.ManualReset);
    
    /// <summary>
    /// This event is set while the index is not being built. It can be used
    /// to be absolutely sure that the index has the most recent state. 
    /// </summary>
    public EventWaitHandle IndexNotBuildingEvent { get; } = new(true, EventResetMode.ManualReset);
    
    /// <summary>
    /// Defines the monitor for the building of the index.
    /// This monitor is held to be absolutely sure that only one indexing is happening at the same time. 
    /// </summary>
    public object SyncIndexBuild { get; } = new();

    /// <summary>
    /// Gets the current index store. This method will wait until the index is built.
    /// </summary>
    /// <returns>The current type index data</returns>
    public TypeIndexData GetCurrentIndexStore()
    {
        IndexBuiltEvent.WaitOne();
        return Current ?? throw new InvalidOperationException("Current is null. This should not happen");
    }
    
    /// <summary>
    /// Waits until the index store is available.
    /// </summary>
    public void WaitForAvailabilityOfIndexStore()
    {
        IndexNotBuildingEvent.WaitOne();
    }
}