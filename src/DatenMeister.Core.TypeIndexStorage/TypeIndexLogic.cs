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
    public TypeIndexStore TypeIndexStore { get;  } = workspaceLogic.ScopeStorage.Get<TypeIndexStore>();

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

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    /// <summary>
    /// Checks, if the index is active. If yes, wait until the index has been updated.
    /// If the index is not active, then return false but also do not wait
    /// </summary>
    /// <returns>true, if the index is up-to-date</returns>
    public bool WaitForUpToDateIfIndexIsActive()
    {
        if (TypeIndexStore.TriggersReceived > 0)
        {
            TypeIndexStore.GetCurrentIndexStore();
            return true;
        }

        return false;
    }

    /// <summary>
    /// In case there is an update of the type index, the method can be called
    /// It starts a listening of 5 seconds and then triggers the update of the index
    /// in case no other call has been requested
    /// </summary>
    public async Task TriggerUpdateOfIndex(bool firstRun = false)
    {
        lock (TypeIndexStore.IndexIsUpToDateEvent)
        {
            TypeIndexStore.IncrementTriggers();
            TypeIndexStore.LastTriggerTime = DateTime.Now;
                
            // Check, if the index is currently in build up
            if (!TypeIndexStore.IndexIsUpToDateEvent.WaitOne(0))
            {
                Logger.Info("We received a trigger, but the index is already being built.");
                
                // We reset the event, so we are sure 
                TypeIndexStore.IndexIsUpToDateEvent.Reset();
                TypeIndexStore.TriggerOccuredDuringIndexing = true;
                return;
            }
            // First, mark that we are updating the index
            TypeIndexStore.IndexIsUpToDateEvent.Reset();
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
                if (TypeIndexStore.TriggerOccuredDuringIndexing)
                {
                    TypeIndexStore.TriggerOccuredDuringIndexing = false;
                    // Do it again
                    continue;
                }

                TypeIndexStore.IndexIsUpToDateEvent.Set();

                break;
            }
        }
        else
        {
            // In case we are not in the first run, we trigger the 5-second time period
            TypeIndexStore.LastTriggerTime = DateTime.Now;

            var token = _cancellationTokenSource.Token;
            _ = Task.Run(async () =>
            {
                try{

                    while (true)
                    {
                        TimeSpan timeToWait;
                        lock (TypeIndexStore.IndexIsUpToDateEvent)
                        {
                            var elapsed = DateTime.Now - TypeIndexStore.LastTriggerTime;
                            if (elapsed >= TypeIndexStore.IndexWaitTime)
                            {
                                // Quiet period finished, proceed to execution
                                break;
                            }

                            timeToWait = TypeIndexStore.IndexWaitTime - elapsed + TimeSpan.FromMilliseconds(100);
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
                    // Task was cancelled, so we completely quit
                    return;
                }

                // Now run the indexing until the trigger has not been run
                while (true)
                {
                    PerformIndexing();
                    if (!TypeIndexStore.TriggerOccuredDuringIndexing)
                    {
                        TypeIndexStore.TriggerOccuredDuringIndexing = false;
                        break;
                    }
                }

                // Only, if everything is done, we set the indexing
                TypeIndexStore.IndexIsUpToDateEvent.Set();
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
                    async (changedWorkspace, _, _) =>
                    {
                        Logger.Info($"Change in workspace {changedWorkspace.id} detected");
                        await TriggerUpdateOfIndex();
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
        
        // We also reset the information that the index is being built since it might be out of date
        TypeIndexStore.IndexIsUpToDateEvent.Reset();
    }

    /// <summary>
    /// Executes the indexing process for type data within the configured workspaces.
    /// This method builds the type index data by processing workspace information and updates the current index state.
    /// It is invoked internally during index updates and manages synchronization to ensure thread-safe index building.
    /// </summary>
    public void PerformIndexing()
    {
        using var stopWatchLogger = new StopWatchLogger(Logger,
            $"Performing indexing. Count: {TypeIndexStore.NumberOfReindexes}");
        
        var storage = TypeIndexStore;
        lock (storage.SyncIndexBuild)
        {
            // First, reset the trigger. It may have been set before 
            TypeIndexStore.TriggerOccuredDuringIndexing = false;
            
            // Add statistical information being used for testing 
            TypeIndexStore.IncrementReindexes();
            
            // Now, create the new index and swap it
            storage.Next = new TypeIndexData();

            if (!storage.IndexFirstBuiltEvent.WaitOne(0))
            {
                // In case we are doing the very first indexing, we need to build the dependency graph first
                Logger.Info("Building workspace dependency graph for the first time");
                
                BuildWorkspaceDependencyGraph(storage.Next);
                SwapNextAndCurrentTypeData();
                
                storage.Next = new TypeIndexData();
            }

            // But redo it to simplify logic
            BuildWorkspace(storage.Next);            
            SwapNextAndCurrentTypeData();
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
            BuildWorkspaceDependencyGraph(indexData);
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
                        AddClassesToWorkspace(workspaceModel, (extent as IUriExtent)?.contextURI() ?? string.Empty, element);
                    }
                }
                
                AddInheritedAttributesFromGeneralizations(workspaceModel);
            }
        }
    }

    private void BuildWorkspaceDependencyGraph(TypeIndexData indexData)
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

        // Checks whether the workspaces are neighbors
        foreach (var workspaceModel in indexData.Workspaces)
        {
            foreach (var otherWorkspaceModel in indexData.Workspaces.Where(x=> x != workspaceModel))
            {
                if (!IsMetaWorkspace(workspaceModel, otherWorkspaceModel)
                    && !IsMetaWorkspace(otherWorkspaceModel, workspaceModel))
                {
                    workspaceModel.NeighborWorkspaces.Add(otherWorkspaceModel.WorkspaceId);
                }
            }
        }
            
            
        // Now we need to understand the neighboring workspaces which are not dependent to each other
        // A workspace is dependent to each other in case it has a metaworkspace (one level or multiple)
        // or is a metaworkspace of the other
        bool IsMetaWorkspace(WorkspaceModel focus, WorkspaceModel other, HashSet<string>? visited = null)
        {
            // Infinite Loop breaker! 
            visited ??= new HashSet<string>();
            if (!visited.Add(focus.WorkspaceId))
            {
                return false;
            }
                
            // First, check that the source is not a direct or indirect workspace of the target
            if (focus.MetaclassWorkspaces.Any(x => x == other.WorkspaceId))
            {
                return true;
            }

            // Checks recursively that the workspace is might be contained by that workspace
            foreach (var metaWorkspace in focus.MetaclassWorkspaces)
            {
                var metaWorkspaceOfFocus = indexData.FindWorkspace(metaWorkspace);
                if (metaWorkspaceOfFocus != null && IsMetaWorkspace(metaWorkspaceOfFocus, other, visited))
                {
                    return true;
                }
            }

            return false;
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
    /// <param name="extentUri">The URI of the extent being evaluated</param>
    /// <param name="element">Elements which is to be evaluated</param>
    private void AddClassesToWorkspace(WorkspaceModel workspaceModel, string extentUri, IElement element)
    {
        // Check, if we are a package, if yes, then we look into its properties
        if (element.isSet(_UML._Packages._Package.packagedElement))
        {
            // Ok, we have packaged element, gets all of them and return them
            var packagedElements = 
                element.getOrDefault<IReflectiveCollection>(_UML._Packages._Package.packagedElement)
                    .OfType<IElement>().ToList();
            foreach (var packagedElement in packagedElements)
            {
                AddClassesToWorkspace(workspaceModel, extentUri, packagedElement);
            }
        }
        
        // Check, if we are a classifier
        if (element.getMetaClass()?.equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
        {
            var classModel = CreateClassModel(element);
            classModel.ExtentUri = extentUri;
            classModel.WorkspaceId = workspaceModel.WorkspaceId;
            workspaceModel.ClassModels.Add(classModel);
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
            Uri = (element as IKnowsUri)?.Uri ?? string.Empty,
            CachedElement = element
        };

        // Get th Generalizations of the class
        var generalizations =
            element.getOrDefault<IReflectiveCollection?>(_UML._StructuredClassifiers._Class.generalization)
                ?.ToList();
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
        var ownedAttributes =
            element.getOrDefault<IReflectiveCollection?>(
                    _UML._StructuredClassifiers._Class.ownedAttribute)
                ?.ToList();
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

        if (attribute.isSet(_UML._Classification._Property.upperValue))
        {
            var multiplier = attribute.getOrDefault<string>(_UML._Classification._Property.upperValue);
            attributeModel.IsMultiple = multiplier is not ("0" or "1");
        }

        if (attribute.isSet(_UML._Classification._Property.upper))
        {
            var multiplier = attribute.getOrDefault<string>(_UML._Classification._Property.upper);
            attributeModel.IsMultiple = multiplier is not ("0" or "1");
        }

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
            storage.IndexFirstBuiltEvent.Set();
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