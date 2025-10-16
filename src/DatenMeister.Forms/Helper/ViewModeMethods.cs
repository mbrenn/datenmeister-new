using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Forms.Helper;

public class ViewModeMethods(IWorkspaceLogic workspaceLogic)
{
    /// <summary>
    /// Gets all available view modes which are stored within the management workspace
    /// </summary>
    /// <returns>Enumeration of view modes. These elements are of type 'ViewMode'</returns>
    public IEnumerable<IObject> GetViewModes()
    {
        var managementWorkspace = workspaceLogic.GetManagementWorkspace();
        return managementWorkspace.GetAllDescendentsOfType(_Forms.TheOne.__ViewMode)
            .OfType<IObject>();
    }

    /// <summary>
    ///     Gets the default view mode for a certain object by querying the view mode instances as
    ///     given in the management workspace
    /// </summary>
    /// <param name="extent">Extent whose view mode is requested</param>
    /// <returns>Found element or null if not found</returns>
    public IElement? GetDefaultViewMode(IExtent? extent)
    {
        var managementWorkspace = workspaceLogic.GetManagementWorkspace();

        var extentTypes = extent?.GetConfiguration()?.ExtentTypes;
        if (extentTypes != null)
        {
            foreach (var extentType in extentTypes)
            {
                var result = managementWorkspace
                    .GetAllDescendentsOfType(_Forms.TheOne.__ViewMode)
                    .WhenPropertyHasValue(_Forms._ViewMode.defaultExtentType, extentType)
                    .OfType<IElement>()
                    .FirstOrDefault();
                if (result != null) return result;
            }
        }

        return managementWorkspace
            .GetAllDescendentsOfType(_Forms.TheOne.__ViewMode)
            .WhenPropertyHasValue(_Forms._ViewMode.id, ViewModes.Default)
            .OfType<IElement>()
            .FirstOrDefault();
    }
}