namespace DatenMeister.Core.TypeIndexAssembly;

/// <summary>
/// This class stores the permanent information for the TypeIndexStorage.
/// It contains the data itself, but in addition also synchronization mechanisms
/// so the type index can be updated offline
/// </summary>
public class TypeIndexStore
{
    /// <summary>
    /// Stores the number of triggers that had been received.
    /// </summary>
    private int _numberOfTriggersReceived;
    
    /// <summary>
    /// Stores the number of re-indexes that had been received.
    /// </summary>
    private int _numberOrReIndexes;

    /// <summary>
    /// Defines the index wait time after which the storage will be updated
    /// </summary>
    public TimeSpan IndexWaitTime { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets or sets the number of update-triggers that had been received.
    /// This is mainly used for testing purposes
    /// </summary>
    public int TriggersReceived
    {
        get => _numberOfTriggersReceived;
        set => _numberOfTriggersReceived = value;
    }

    /// <summary>
    /// Increment the number of update-triggers that had been received.
    /// </summary>
    public void IncrementTriggers()
    {
        Interlocked.Increment(ref _numberOfTriggersReceived);
    }

    /// <summary>
    /// Gets or sets the number of update-triggers that had been received.
    /// This is mainly used for testing purposes
    /// </summary>
    public int NumberOfReindexes
    {
        get => _numberOrReIndexes;
        set => _numberOrReIndexes = value;
    }

    /// <summary>
    /// Increment the number of update-triggers that had been received.
    /// </summary>
    public void IncrementReindexes()
    {
        Interlocked.Increment(ref _numberOrReIndexes);
    }
    
    /// <summary>
    /// Gets or sets the date when the last trigger for the update was being requested
    /// </summary>
    public DateTime LastTriggerTime { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the value whether a trigger to update the model occured during the indexing
    /// </summary>
    public bool TriggerOccuredDuringIndexing { get; set; }
    
    /// <summary>
    /// Gets or sets the current data for the TypeIndexing
    /// </summary>
    public TypeIndexData? Current { get; set; }
    
    /// <summary>
    /// Gets or sets the next data for the TypeIndexing which will be swapped to the current data
    /// </summary>
    internal TypeIndexData? Next { get; set; }
    
    /// <summary>
    /// Defines the monitor for the swapping if the index data
    /// </summary>
    public object SyncIndexSwapping { get; } = new();
    
    /// <summary>
    /// This event is set, when the index is finished to be built. 
    /// </summary>
    public EventWaitHandle IndexFirstBuiltEvent { get; } = new(false, EventResetMode.ManualReset);
    
    /// <summary>
    /// This event is set while the index is not being built and has been updated. It can be used
    /// to be absolutely sure that the index has the most recent state. 
    /// </summary>
    public EventWaitHandle IndexIsUpToDateEvent { get; } = new(true, EventResetMode.ManualReset);
    
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
        IndexFirstBuiltEvent.WaitOne();
        return Current ?? throw new InvalidOperationException("Current is null. This should not happen");
    }
    
    /// <summary>
    /// Waits until the index store is available.
    /// </summary>
    public void WaitForAvailabilityOfIndexStore()
    {
        IndexIsUpToDateEvent.WaitOne();
    }
}