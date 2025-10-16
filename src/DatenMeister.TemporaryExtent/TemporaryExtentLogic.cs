using System.Collections.Concurrent;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.TemporaryExtent;

/// <summary>
/// Defines some helper methods for the temporary extent plugin
/// </summary>
public class TemporaryExtentLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    public const string InternalTempUri = "dm:///_internal/temp";

    /// <summary>
    /// Defines the logger
    /// </summary>
    private static readonly ILogger ClassLogger = new ClassLogger(typeof(TemporaryExtentLogic));

    public static TimeSpan DefaultCleanupTime { get; set; } = TimeSpan.FromHours(1);


    /// <summary>
    /// Gets the name of the workspace
    /// </summary>
    public IWorkspace Workspace => workspaceLogic.GetDataWorkspace();

    /// <summary>
    /// Gets the name of the workspace
    /// </summary>
    public string WorkspaceName => Workspace.id;

    /// <summary>
    /// Maps the element to a datetime until when it shall be deleted.
    /// If the element is not found here, then it will be directly deleted
    /// </summary>
    private static readonly ConcurrentDictionary<IObject, DateTime> ElementMapping = new ();

    /// <summary>
    /// Gets the temporary extent and creates a new one, if necessary
    /// </summary>
    public IUriExtent TemporaryExtent
    {
        get
        {
            if (workspaceLogic.FindExtent(WorkspaceName, TemporaryExtentPlugin.Uri) 
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
        return workspaceLogic.FindExtent(WorkspaceName, TemporaryExtentPlugin.Uri);
    }

    /// <summary>
    /// Creates a simple temporary element and adds it to the temporary extent
    /// </summary>
    /// <param name="metaClass">Metaclass to be used</param>
    /// <param name="cleanUpTime">Defines the cleanup time for the given item.
    /// If this is not set, then the default time is taken</param>
    /// <param name="addToExtent">true, if the created element shall be added to the extent</param>
    /// <returns>The created element itself</returns>
    public IElement CreateTemporaryElement(IElement? metaClass, TimeSpan? cleanUpTime = null, bool addToExtent = true)
    {
        var foundExtent = TemporaryExtent;

        var created = MofFactory.CreateElement(foundExtent, metaClass);
        ElementMapping[created] = DateTime.Now + (cleanUpTime ?? DefaultCleanupTime);
        if (addToExtent)
        {
            foundExtent.elements().add(created);
        }

        return created;
    }

    /// <summary>
    /// Creates a temporary element within the temporary extent.
    /// </summary>
    /// <param name="metaClassUri">The Uri of the metaclass being used to add the element</param>
    /// <param name="cleanUpTime">The cleanup time after which the element will be removed automatically. Default is null.</param>
    /// <param name="addToExtent">If true, the element will be added to the temporary extent. Default is true.</param>
    /// <returns>The created temporary element.</returns>
    public IElement CreateTemporaryElementByUri(string metaClassUri, TimeSpan? cleanUpTime = null, bool addToExtent = true)
    {
        var result = CreateTemporaryElement((IElement?)null, cleanUpTime, addToExtent);

        if (!string.IsNullOrEmpty(metaClassUri))
        {
            (result as MofElement
             ?? throw new InvalidOperationException("Created item does not support setting of metaclass"))
                .SetMetaClass(metaClassUri);
        }

        return result;
    }

    public void CleanElements()
    {
        var foundExtent = TemporaryExtent;

        var currentTime = DateTime.Now;

        // Go through the elements and collect these ones whose clean up time has passed
        var itemsToBeDeleted = 
            foundExtent.elements()
                .OfType<IObject>()
                .Where(element =>
                {
                    if (ElementMapping.TryGetValue(element, out var time))
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
            ElementMapping.Remove(element, out _);
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
        var extent = new MofUriExtent(temporaryProvider, InternalTempUri, scopeStorage);
        workspaceLogic.AddExtent(Workspace, extent);
        return extent;
    }
}