using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView;

public class DataViewHelper(IWorkspaceLogic workspaceLogic)
{
    public IElement CreateDataview(string name, string extentUri)
    {
        var viewExtent = workspaceLogic.GetUserFormsExtent();
        var metaClass = _DatenMeister.TheOne.DataViews.__DataView;
        var createdElement = new MofFactory(viewExtent).create(metaClass);

        createdElement.set(_DatenMeister._DataViews._DataView.name, name);
        createdElement.set(_DatenMeister._DataViews._DataView.uri, extentUri);

        viewExtent.elements().add(createdElement);

        return createdElement;
    }

    /// <summary>
    /// Gets the extent for the user views which is usually used to define the views
    /// </summary>
    /// <returns>Extent containing the user views</returns>
    public IUriExtent GetUserFormExtent() =>
        workspaceLogic.GetUserFormsExtent();

    public Workspace GetViewWorkspace() =>
        workspaceLogic.GetViewsWorkspace();
}