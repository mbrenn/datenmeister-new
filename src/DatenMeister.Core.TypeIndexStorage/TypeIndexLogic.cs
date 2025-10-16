using DatenMeister.Core.Interfaces;

namespace DatenMeister.Core.TypeIndexAssembly;

public class TypeIndexLogic(IScopeStorage scopeStorage)
{
    /// <summary>
    /// Stores the scope storage
    /// </summary>
    private IScopeStorage ScopeStorage { get; set; } = scopeStorage;
    
    /// <summary>
    /// Called by the plugin after all the types have been loaded.
    /// It starts the indexing the first time and will return a 
    /// task which is performing the indexing
    /// </summary>
    public async Task CreateIndexFirstTime()
    {
    }
    
    public TimeSpan IndexWaitTime { get; set; } = TimeSpan.FromSeconds(5);
    
    private TimeSpan _lastIndexTime = TimeSpan.Zero;
    
    private TimeSpan _lastTriggerTime = TimeSpan.Zero;
    
    private bool _isIndexing = false;
    
    private bool _triggerOccuredDuringIndexing = false;

    /// <summary>
    /// In case there is an update of the type index, the method can be called
    /// It starts a listening of 5 seconds and then triggers the update of the index
    /// in case no other call has been requested
    /// </summary>
    public async Task TriggerUpdateOfIndex()
    {
        while (true)
        {
            if (_isIndexing)
            {
                _triggerOccuredDuringIndexing = true;
                return;
            }

            // Perform the updates

            // After the updates are performed, check whether the trigger has been called during the indexing
            if (_triggerOccuredDuringIndexing)
            {
                _triggerOccuredDuringIndexing = false;
                // Do it again
                continue;
            }

            break;
        }
    }

    /// <summary>
    /// This method will be called before requesting the first indexing.
    /// It adds the listening of changes within potential type extents
    /// </summary>
    public async Task StartListening()
    {
        
    }

    /// <summary>
    /// This method will be called when the application is shutting down.
    /// </summary>
    public async Task StopListening()
    {
        
    }
}