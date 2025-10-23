using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView;

public class DataViewHelper(IWorkspaceLogic workspaceLogic)
{
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

    public IWorkspace GetViewWorkspace() =>
        workspaceLogic.GetViewsWorkspace();
}