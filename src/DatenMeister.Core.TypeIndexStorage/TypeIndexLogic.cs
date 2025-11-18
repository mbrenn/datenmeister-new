using BurnSystems.Logging;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.TypeIndexAssembly.Helper;
using DatenMeister.Core.TypeIndexAssembly.Model;

namespace DatenMeister.Core.TypeIndexAssembly;

/// <summary>
/// Handles logic for type indexing within specified workspaces.
/// Facilitates the management, updating, and querying of indexed types,
/// including interactions with workspace and metadata for type resolution.
/// </summary>
public class TypeIndexLogic(IWorkspaceLogic workspaceLogic)
{
    private static readonly ILogger Logger = new ClassLogger(typeof(TypeIndexLogic));

    /// <summary>
    /// Stores the scope storage
    /// </summary>
    private IScopeStorage ScopeStorage { get; set; } 
        = workspaceLogic.ScopeStorage ?? throw new InvalidOperationException("ScopeStorage is null");
    
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

        await TriggerUpdateOfIndex(true);
    }

    public TimeSpan IndexWaitTime { get; set; } = TimeSpan.FromSeconds(5);
    
    private DateTime _lastIndexTime = DateTime.MinValue;
    
    private DateTime _lastTriggerTime = DateTime.MinValue;
    
    private bool _triggerOccuredDuringIndexing;

    private CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// In case there is an update of the type index, the method can be called
    /// It starts a listening of 5 seconds and then triggers the update of the index
    /// in case no other call has been requested
    /// </summary>
    private async Task TriggerUpdateOfIndex(bool firstRun = false)
    {
        lock (TypeIndexStore.IndexNotBuildingEvent)
        {
            if (!TypeIndexStore.IndexNotBuildingEvent.WaitOne(0))
            {
                Logger.Info("We received a trigger, but the index is already being built.");
                
                // We reset the event, so we are sure 
                TypeIndexStore.IndexNotBuildingEvent.Reset();
                _triggerOccuredDuringIndexing = true;
                return;
            }
            
            // Now, we know that we can trigger the update itself.
            
            // First, mark that we are updating the index
            TypeIndexStore.IndexNotBuildingEvent.Reset();
            _lastTriggerTime = DateTime.Now;
        }
        
        // We leave the lock here, because we are protected now and can request the updating. 
        // This is in case we are heaving the first run
        if (firstRun)
        {
            while (true)
            {
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

                TypeIndexStore.IndexNotBuildingEvent.Set();

                break;
            }
        }
        else
        {
            // In case we are not in the first run, we trigger the 5-second time period
            _lastTriggerTime = DateTime.Now;

            var token = _cancellationTokenSource.Token;
            _ = Task.Run(async () =>
            {
                try{

                    while (true)
                    {
                        TimeSpan timeToWait;
                        lock (TypeIndexStore.IndexNotBuildingEvent)
                        {
                            var elapsed = DateTime.Now - _lastTriggerTime;
                            if (elapsed >= IndexWaitTime)
                            {
                                // Quiet period finished, proceed to execution
                                break;
                            }

                            timeToWait = IndexWaitTime - elapsed;
                        }

                        Logger.Info($"Waiting for {timeToWait.TotalSeconds} seconds");

                        await Task.Delay(timeToWait, token);
                        if (token.IsCancellationRequested)
                        {
                            Logger.Info("Cancellation requested");
                            return;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Task was cancelled, which is expected
                    return;
                }

                // Now run the indexing until the trigger has not been run
                while (true)
                {
                    PerformIndexing();
                    if (!_triggerOccuredDuringIndexing)
                    {
                        _triggerOccuredDuringIndexing = false;
                        break;
                    }
                }

                // Only, if everything is done, we set the indexing
                TypeIndexStore.IndexNotBuildingEvent.Set();
            }, token);

        }
    }

    /// <summary>
    /// This method will be called before requesting the first indexing.
    /// It adds the listening of changes within potential type extents
    /// </summary>
    public async Task StartListening()
    {
        // Go through all known workspaces which are having metaworkspaces
        await Task.Run(() =>
        {
            TypeIndexStore.WaitForAvailabilityOfIndexStore();

            Logger.Info("Start listening for changes");
            foreach (var workspace in
                     TypeIndexStore.Current?.Workspaces.Where(x =>
                         TypeIndexStore.Current.IsWorkspaceMetaClass(x.WorkspaceId))
                     ?? throw new InvalidOperationException("Current is null"))
            {
                WorkspaceLogic.ChangeEventManager.RegisterFor(
                    WorkspaceLogic.GetWorkspace(workspace.WorkspaceId)
                    ?? throw new InvalidOperationException("Workspace not found"),
                    (changedWorkspace, changedExtent, changedElement) =>
                    {
                        Logger.Info($"Change in workspace {changedWorkspace.id} detected");
                    }
                );
            }
        });
    }

    /// <summary>
    /// This method will be called when the application is shutting down.
    /// </summary>
    public void StopListening()
    {
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Executes the indexing process for type data within the configured workspaces.
    /// This method builds the type index data by processing workspace information and updates the current index state.
    /// It is invoked internally during index updates and manages synchronization to ensure thread-safe index building.
    /// </summary>
    private void PerformIndexing()
    {
        using var stopWatchLogger = new StopWatchLogger(Logger, "Performing indexing");
        
        var storage = TypeIndexStore;
        lock (storage.SyncIndexBuild)
        {
            storage.Next = new TypeIndexData();
            BuildWorkspace(storage.Next);
            _lastIndexTime = DateTime.Now;
            
            SwapNextAndCurrentTypeData();
            
            storage.IndexBuiltEvent.Set();
        }
    }

    /// <summary>
    /// Builds the workspace dependency tree
    /// </summary>
    /// <param name="indexData">Indexdata where the update will be stored</param>
    private void BuildWorkspace(TypeIndexData indexData)
    {
        using (var _ = new StopWatchLogger(Logger, "Building workspace dependency tree"))
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
        }

        using (var _ = new StopWatchLogger(Logger, "Building classes"))
        {
            // Now, we build the information about the metaclasses
            foreach (var workspace in WorkspaceLogic.Workspaces)
            {
                if (!indexData.IsWorkspaceMetaClass(workspace.id))
                {
                    // This is not a metaclass workspace
                    continue;
                }

                using var __ = new StopWatchLogger(Logger, $"Building classes within workspace: {workspace.id}");

                // Now we go through the metaclasses
                var workspaceModel = indexData.GetWorkspace(workspace.id)
                                     ?? throw new InvalidOperationException("Workspace not found");
                foreach (var extent in workspace.extent)
                {
                    Logger.Info(
                        $"Building class within extent: {(extent as IUriExtent)?.contextURI() ?? "Unknown Uri"}");

                    // Adds the extent to the workspace
                    var extentInfo = new ExtentModel
                    {
                        Uri = (extent as IUriExtent)?.contextURI() ?? string.Empty
                    };
                    
                    workspaceModel.Extents.Add(extentInfo);
                    
                    // Now we go through the elements and try to find the classes
                    foreach (var element in extent.elements().OfType<IElement>())
                    {
                        AddClassesToWorkspace(workspaceModel, element);
                    }
                }
                
                AddInheritedAttributesFromGeneralizations(workspaceModel);
            }
        }
    }

    /// <summary>
    /// Adds inherited attributes to classes within the specified workspace model
    /// by traversing their generalizations. For each generalization, attributes
    /// from the generalized class are copied and marked as inherited within the
    /// derived class.
    /// </summary>
    /// <param name="workspaceModel">
    /// The workspace model containing the classes and their generalizations to process.
    /// </param>
    private static void AddInheritedAttributesFromGeneralizations(WorkspaceModel workspaceModel)
    {
        // After we are done with the complete workspace, we add the attributes from the generalizations
        foreach (var classModel in workspaceModel.ClassModels)
        {
            foreach (var generalization in classModel.Generalizations)
            {
                var generalizedClassModel = workspaceModel.FindClassByUri(generalization);
                if (generalizedClassModel != null)
                {
                    foreach (var attribute in generalizedClassModel.Attributes)
                    {
                        classModel.Attributes.Add(attribute with {IsInherited = true});
                    }
                }
            }
        }
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
            var packagedElements = 
                element.getOrDefault<IReflectiveCollection>(_UML._Packages._Package.packagedElement).OfType<IElement>();
            foreach (var packagedElement in packagedElements)
            {
                AddClassesToWorkspace(workspaceModel, packagedElement);
            }
        }
        
        // Check, if we are a classifier
        if (element.getMetaClass()?.equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
        {
            workspaceModel.ClassModels.Add(CreateClassModel(element));
        }
    }

    /// <summary>
    /// Adds the specific class to the workspacemodel, so it can be easily looked up. 
    /// </summary>
    /// <param name="element">The element reflecting the class</param>
    private ClassModel CreateClassModel(IElement element)
    {
        // Add the class itself
        var classModel = new ClassModel
        {
            Id = (element as IHasId)?.Id ?? string.Empty,
            Name = element.getOrDefault<string>(_UML._StructuredClassifiers._Class.name),
            FullName = NamedElementMethods.GetFullName(element),
            Uri = (element as IKnowsUri)?.Uri ?? string.Empty
        };
        
        // Get th Generalizations of the class
        var generalizations = element.getOrDefault<IReflectiveCollection?>(_UML._StructuredClassifiers._Class.generalization);
        if (generalizations != null)
        {
            foreach (var generalization in generalizations.OfType<IElement>())
            {
                var general = generalization.getOrDefault<IElement?>(_UML._Classification._Generalization.general);
                if (general is IKnowsUri uriGeneral)
                {
                    classModel.Generalizations.Add(uriGeneral.Uri);
                }
            }
        }

        // Now get the properties / attributes
        var ownedAttributes = element.getOrDefault<IReflectiveCollection?>(
            _UML._StructuredClassifiers._Class.ownedAttribute);
        if (ownedAttributes != null)
        {
            foreach (var attribute in ownedAttributes.OfType<IElement>())
            {
                classModel.Attributes.Add(CreateAttributeModel(attribute));
            }
        }

        return classModel;
    }

    /// <summary>
    /// Creates an attribute model out the attributes of an UML attribute
    /// </summary>
    /// <param name="attribute">Attribute to be evaluated</param>
    /// <returns>The created attribute</returns>
    private AttributeModel CreateAttributeModel(IElement attribute)
    {
        var attributeModel = new AttributeModel
        {
            Name = attribute.getOrDefault<string>(_UML._Classification._Property.name),
            TypeUrl =
                (attribute.getOrDefault<IElement>(_UML._Classification._Property.type) as IKnowsUri)
                ?.Uri ?? string.Empty,
            Url = (attribute as IKnowsUri)?.Uri ?? string.Empty,
            Id = (attribute as IHasId)?.Id ?? string.Empty
        };
        
        var multiplier = attribute.getOrDefault<int>(_UML._Classification._Property.upperValue);
        attributeModel.IsMultiple = multiplier is not (0 or 1);

        var defaultValue = attribute.getOrDefault<object?>(_UML._Classification._Property.defaultValue);
        if (defaultValue != null)
        {
            attributeModel.DefaultValue = defaultValue;
        }
        
        var isComposite = attribute.getOrDefault<bool>(_UML._Classification._Property.isComposite);
        if (isComposite)
        {
            attributeModel.IsComposite = true;
        }
        else
        {
            var typeUrl = attributeModel.TypeUrl;
            if (typeUrl == _PrimitiveTypes.TheOne.__String.Uri
                || typeUrl == _PrimitiveTypes.TheOne.__Integer.Uri
                || typeUrl == _PrimitiveTypes.TheOne.__Boolean.Uri
                || typeUrl == _PrimitiveTypes.TheOne.__Real.Uri
                || typeUrl == _PrimitiveTypes.TheOne.__UnlimitedNatural.Uri
                || typeUrl == Models._CommonTypes.TheOne.__DateTime.Uri)
            {
                attributeModel.IsComposite = true;
            }
        }
        
        return attributeModel;
        
    }

    /// <summary>
    /// Swaps the active information of the TypeIndex after the index has been built up with the new.
    /// This method is used to ensure that there is always one active typeindex available in case of an
    /// update of the type information and while performing the re-index. 
    /// </summary>
    private void SwapNextAndCurrentTypeData()
    {
        var storage = TypeIndexStore;
        lock (storage.SyncIndexSwapping)
        {
            storage.Current = storage.Next;
            storage.Next = null;
            storage.IndexBuiltEvent.Set();
        }
    }

    /// <summary>
    /// Finds the class model by the given url within one of the meta workspaces of that given workspace
    /// </summary>
    /// <param name="workspace">Workspace to be evaluated</param>
    /// <param name="url">Url to be checked</param>
    /// <returns>The found class model or null, if not found</returns>
    public ClassModel? FindClassModelByUrlWithinMetaWorkspaces(string workspace, string url)
    {
        var workspaceModel = TypeIndexStore.Current?.FindWorkspace(workspace);
        var metaclassWorkspaces = workspaceModel?.MetaclassWorkspaces;
        return metaclassWorkspaces
            ?.Select(x => TypeIndexStore.Current?.FindWorkspace(x))
            .Select(x => x?.FindClassByUri(url))
            .FirstOrDefault(x => x != null);
    }

    /// <summary>
    /// Finds the class model by the given url within one of the meta workspaces of that given workspace
    /// </summary>
    /// <param name="workspace">Workspace to be evaluated</param>
    /// <param name="url">Url to be checked</param>
    /// <returns>The found class model or null, if not found</returns>
    public ClassModel? FindClassModelByUrlWithinWorkspace(string workspace, string url)
    {
        var workspaceModel = TypeIndexStore.Current?.FindWorkspace(workspace);
        return workspaceModel?.FindClassByUri(url);
    }
}