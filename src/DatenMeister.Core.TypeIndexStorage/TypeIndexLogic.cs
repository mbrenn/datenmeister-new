using System.Formats.Asn1;
using BurnSystems.Logging;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.TypeIndexAssembly.Model;

namespace DatenMeister.Core.TypeIndexAssembly;

public class TypeIndexLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    private static readonly ILogger Logger = new ClassLogger(typeof(TypeIndexLogic));
    
    /// <summary>
    /// Stores the scope storage
    /// </summary>
    private IScopeStorage ScopeStorage { get; set; } = scopeStorage;
    
    /// <summary>
    /// Stores the workspace logic
    /// </summary>
    private IWorkspaceLogic WorkspaceLogic { get; set; } = workspaceLogic;

    /// <summary>
    /// Gets the TypeIndexStore
    /// </summary>
    private TypeIndexStore TypeIndexStore => ScopeStorage.Get<TypeIndexStore>();

    /// <summary>
    /// Called by the plugin after all the types have been loaded.
    /// It starts the indexing the first time and will return a 
    /// task which is performing the indexing
    /// </summary>
    public async Task CreateIndexFirstTime()
    {
        Logger.Info("Trigger first indexing");

        await TriggerUpdateOfIndex();
    }

    public TimeSpan IndexWaitTime { get; set; } = TimeSpan.FromSeconds(5);
    
    private DateTime _lastIndexTime = DateTime.MinValue;
    
    private DateTime _lastTriggerTime = DateTime.MinValue;
    
    private bool _isIndexing = false;
    
    private bool _triggerOccuredDuringIndexing = false;

    /// <summary>
    /// In case there is an update of the type index, the method can be called
    /// It starts a listening of 5 seconds and then triggers the update of the index
    /// in case no other call has been requested
    /// </summary>
    private async Task TriggerUpdateOfIndex()
    {
        while (true)
        {
            if (_isIndexing)
            {
                _triggerOccuredDuringIndexing = true;
                return;
            }

            // Perform the updates
            Logger.Info("Trigger update of index");
            await Task.Run(PerformIndexing);

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
    public void StartListening()
    {
        // Gets notified in case the Workspace Logic
    }

    /// <summary>
    /// This method will be called when the application is shutting down.
    /// </summary>
    public void StopListening()
    {
        
    }
    
    private void PerformIndexing()
    {
        using var stopWatchLogger = new StopWatchLogger(Logger, "Performing indexing");
        
        var storage = TypeIndexStore;
        lock (storage.SyncIndexBuild)
        {
            _isIndexing = true;
            storage.Next = new TypeIndexData();

            BuildWorkspace(storage.Next);

            _isIndexing = false;
            _lastIndexTime = DateTime.Now;
            
            SwapNextAndCurrentTypeData();
        }
    }

    /// <summary>
    /// Builds the workspace dependency tree
    /// </summary>
    /// <param name="indexData">Indexdata where the update will be stored</param>
    private void BuildWorkspace(TypeIndexData indexData)
    {
        // First, we build the workspace dependency tree
        foreach (var workspace in WorkspaceLogic.Workspaces)
        {
            var model = new WorkspaceModel
            {
                WorkspaceId = workspace.id
            };

            foreach (var meta in workspace.MetaWorkspaces)
            {
                model.MetaclassWorkspaces.Add(meta.id);
            }
            
            indexData.Workspaces.Add(model);
        }
        
        // Now, we build the information about the metaclasses
        foreach (var workspace in WorkspaceLogic.Workspaces)
        {
            if (!indexData.IsWorkspaceMetaClass(workspace.id))
            {
                // This is not a metaclass workspace
                continue;
            }
            
            Logger.Info("Building classes within workspace: " + workspace.id);
            
            // Now we go through the metaclasses
            var workspaceModel = indexData.GetWorkspace(workspace.id)
                ?? throw new InvalidOperationException("Workspace not found");
            foreach (var extent in workspace.extent)
            {
                foreach (var element in extent.elements().OfType<IElement>())
                {
                    AddClassesToWorkspace(workspaceModel, element);
                }
            }
        }
        
        Logger.Info($"Finished building workspace dependency tree: {indexData.Workspaces.Count} Workspaces indexed");
    }

    /// <summary>
    /// Adds the classes to the workspace
    /// </summary>
    /// <param name="workspaceModel">Model in which the classes will be stored</param>
    /// <param name="element">Elements which is to be evaluated</param>
    private void AddClassesToWorkspace(WorkspaceModel workspaceModel, IElement element)
    {
        // Check, if we are a package, if yes, then we look into its properties
        if (element.isSet(_UML._Packages._Package.packagedElement))
        {
            // Ok, we have packaged element, gets all of them and return them
            element.get(_UML._Packages._Package.packagedElement);
        }
    }

    private void SwapNextAndCurrentTypeData()
    {
        var storage = TypeIndexStore;
        lock (storage.SyncIndexSwapping)
        {
            storage.Current = storage.Next;
            storage.Next = null;
        }
    }
}