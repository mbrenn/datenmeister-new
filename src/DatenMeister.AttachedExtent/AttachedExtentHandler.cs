using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using static DatenMeister.Core.Models._DatenMeister._AttachedExtent;

namespace DatenMeister.AttachedExtent;

/// <summary>
/// Defines the handler to support the attached extents. 
/// </summary>
public class AttachedExtentHandler(IWorkspaceLogic workspaceLogic)
{
    public const string AttachedExtentProperty = "DatenMeister.AttachedExtent";

    /// <summary>
    /// Gets the configuration for the attached extent
    /// </summary>
    /// <param name="attachedExtent">Attached extent to be parsed</param>
    /// <returns>The found extent</returns>
    public IElement? GetConfiguration(IExtent attachedExtent)
    {
        var attachedExtentConfiguration = attachedExtent.getOrDefault<IElement>(AttachedExtentProperty);

        return attachedExtentConfiguration;
    }

    public void SetConfiguration(IUriExtent attachedExtent, IElement configuration)
    {
        attachedExtent.set(
            AttachedExtentProperty,
            configuration);
    }

    /// <summary>
    /// Gets the original extent 
    /// </summary>
    /// <param name="attachedExtent">Attached extent which might be connected to an original extent</param>
    /// <returns>Found extent or null if not found</returns>
    public IUriExtent? GetOriginalExtent(IUriExtent attachedExtent)
    {
        var configuration = GetConfiguration(attachedExtent);
        if (configuration == null)
        {
            return null;
        }

        var workspace = configuration.getOrDefault<string>(_AttachedExtentConfiguration.referencedWorkspace);
        var extent = configuration.getOrDefault<string>(_AttachedExtentConfiguration.referencedExtent);
        if (workspace == null || extent == null)
        {
            return null;
        }

        return workspaceLogic.FindExtent(workspace, extent);
    }

    /// <summary>
    /// Finds all attached extents to the original extents. 
    /// </summary>
    /// <param name="originalExtent">The original extent whose attached extents are looked for</param>
    /// <returns></returns>
    public IEnumerable<IUriExtent> FindAttachedExtents(IUriExtent originalExtent)
    {
        var workspaceName = originalExtent.GetWorkspace()?.id ?? string.Empty;
        var extentName = originalExtent.contextURI();

        var foundExtents =
            from workspace in workspaceLogic.Workspaces
            from extent in workspace.extent
            let configuration = GetConfiguration(extent)
            where configuration != null
                  && configuration.getOrDefault<string>(
                      _AttachedExtentConfiguration.referencedWorkspace) ==
                  workspaceName
                  && configuration.getOrDefault<string>(
                      _AttachedExtentConfiguration.referencedExtent) == extentName
            let uriExtent = extent as IUriExtent
            where uriExtent != null
            select uriExtent;

        foreach (var foundExtent in foundExtents)
        {
            yield return foundExtent;
        }
    }

    /// <summary>
    /// Gets or creates the attached item.
    /// If the attached item does not exist, a new item is generated in the attached extent
    /// Otherwise the existing item is returned. 
    /// </summary>
    /// <param name="originalItem">Original item to be evaluated</param>
    /// <param name="attachedExtent">Attached extent to be added</param>
    /// <returns>Gets or created the attached item </returns>
    public IElement GetOrCreateAttachedItem(IElement originalItem, IUriExtent attachedExtent)
    {
        var configuration = GetConfiguration(attachedExtent);
        if (configuration == null ||
            configuration.getOrDefault<string>(_AttachedExtentConfiguration.referenceProperty) == null)
        {
            throw new InvalidOperationException(
                "Attached item cannot ba retrieved since the attached extent does not have a configuration");
        }

        // Now go through the attached extent and look for the attached item
        var foundItem = attachedExtent.elements()
            .GetAllDescendantsIncludingThemselves()
            .WhenPropertyHasValue(
                configuration.getOrDefault<string>(_AttachedExtentConfiguration.referenceProperty), originalItem)
            .OfType<IElement>()
            .FirstOrDefault();

        if (foundItem != null) return foundItem;

        var referenceType =
            configuration.getOrDefault<IElement>(_AttachedExtentConfiguration.referenceType);

        var newItem = MofFactory.CreateElement(attachedExtent, referenceType);
        newItem.set(configuration.getOrDefault<string>(_AttachedExtentConfiguration.referenceProperty),
            originalItem);
        attachedExtent.elements().add(newItem);
        return newItem;
    }
}