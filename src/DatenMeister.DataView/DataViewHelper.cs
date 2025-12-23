using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView;

/// <summary>
/// Provides helper methods for data views
/// </summary>
/// <param name="workspaceLogic">The workspace logic</param>
public class DataViewHelper(IWorkspaceLogic workspaceLogic)
{
    /// <summary>
    /// Creates a new data view
    /// </summary>
    /// <param name="name">Name of the data view</param>
    /// <param name="extentUri">URI of the extent to be created</param>
    /// <returns>The created element defining the data view</returns>
    public IElement CreateDataview(string name, string extentUri)
    {
        var viewExtent = workspaceLogic.GetUserFormsExtent();
        var metaClass = _DataViews.TheOne.__DataView;
        var createdElement = new MofFactory(viewExtent).create(metaClass);

        createdElement.set(_DataViews._DataView.name, name);
        createdElement.set(_DataViews._DataView.uri, extentUri);

        viewExtent.elements().add(createdElement);

        return createdElement;
    }

    /// <summary>
    /// Gets the extent for the user views which is usually used to define the views
    /// </summary>
    /// <returns>Extent containing the user views</returns>
    public IUriExtent GetUserFormExtent() =>
        workspaceLogic.GetUserFormsExtent();

    /// <summary>
    /// Gets the workspace containing the views
    /// </summary>
    /// <returns>The workspace for views</returns>
    public IWorkspace GetViewWorkspace() =>
        workspaceLogic.GetViewsWorkspace();
}