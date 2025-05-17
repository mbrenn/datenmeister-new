using System.Collections.Concurrent;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.TemporaryExtent
{
    /// <summary>
    /// Defines some helper methods for the temporary extent plugin
    /// </summary>
    public class TemporaryExtentLogic
    {
        public const string InternalTempUri = "dm:///_internal/temp";

        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(TemporaryExtentLogic));
        
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public static TimeSpan DefaultCleanupTime { get; set; } = TimeSpan.FromHours(1);


        /// <summary>
        /// Gets the name of the workspace
        /// </summary>
        public Workspace Workspace => _workspaceLogic.GetDataWorkspace();

        /// <summary>
        /// Gets the name of the workspace
        /// </summary>
        public string WorkspaceName => Workspace.id;

        /// <summary>
        /// Maps the element to a datetime until when it shall be deleted.
        /// If the element is not found here, then it will be directly deleted
        /// </summary>
        private static readonly ConcurrentDictionary<string, DateTime> _elementMapping = new ();

        public TemporaryExtentLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        /// <summary>
        /// Gets the temporary extent and creates a new one, if necessary
        /// </summary>
        public IUriExtent TemporaryExtent
        {
            get
            {
                if (_workspaceLogic.FindExtent(WorkspaceName, TemporaryExtentPlugin.Uri) 
                    is not IUriExtent foundExtent)
                {
                    // Somebody deleted the extent... So, we will create a new one
                    ClassLogger.Warn($"Temporary Extent was deleted, we will recreate it");
                    foundExtent = CreateTemporaryExtent();
                }

                return foundExtent;
            }
        }
        

        /// <summary>
        /// Tries to find the temporary extent. May also be null
        /// </summary>
        /// <returns>Found extent or null, if not found</returns>
        public IUriExtent? TryGetTemporaryExtent()
        {
            return _workspaceLogic.FindExtent(WorkspaceName, TemporaryExtentPlugin.Uri)
                as IUriExtent;
        }

        /// <summary>
        /// Creates a simple temporary element and adds it to the temporary extent
        /// </summary>
        /// <param name="metaClass">Metaclass to be used</param>
        /// <param name="cleanUpTime">Defines the cleanup time for the given item.
        /// If this is not set, then the default time is taken</param>
        /// <returns>The created element itself</returns>
        public IElement CreateTemporaryElement(IElement? metaClass, TimeSpan? cleanUpTime = null, bool addToExtent = true)
        {
            var foundExtent = TemporaryExtent;

            var created = MofFactory.CreateElement(foundExtent, metaClass);
            var id = (created as IHasId)?.Id 
                     ?? throw new InvalidOperationException("Element does not has an id");
            _elementMapping[id] = DateTime.Now + (cleanUpTime ?? DefaultCleanupTime);
            if (addToExtent)
            {
                foundExtent.elements().add(created);
            }

            return created;
        }

        public void CleanElements()
        {
            var foundExtent = TemporaryExtent;

            var currentTime = DateTime.Now;

            // Go through the elements and collect these ones whose clean up time has passed
            var itemsToBeDeleted = 
                foundExtent.elements()
                    .OfType<IHasId>()
                    .Where(element =>
                        {
                            var id = element.Id;
                            if (id != null && _elementMapping.TryGetValue(id, out var time))
                            {
                                return time < currentTime;
                            }
                            
                            // If item is not in element-mapping, it will be directly deleted
                            return true;
                        })
                    .ToList();

            // Now delete these items
            foreach (var element in itemsToBeDeleted)
            {
                foundExtent.elements().remove(element);
                var id = element.Id;
                if (id != null)
                {
                    _elementMapping.Remove(id, out _);
                }
            }

            // Logging, if something was deleted
            if (itemsToBeDeleted.Count > 0)
            {
                ClassLogger.Info($"{itemsToBeDeleted.Count} items deleted");
            }
        }

        /// <summary>
        /// Creates the temporary extent and adds it to the workspace logic
        /// The temporary extent will not be added to the loaded Extents
        /// </summary>
        public IUriExtent CreateTemporaryExtent()
        {
            var temporaryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(temporaryProvider, InternalTempUri, _scopeStorage);
            _workspaceLogic.AddExtent(Workspace, extent);
            return extent;
        }
    }
}