using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView;

public class DataViewHelper
{
    private readonly IWorkspaceLogic _workspaceLogic;

    public DataViewHelper(IWorkspaceLogic workspaceLogic)
    {
        _workspaceLogic = workspaceLogic;
    }

    public IElement CreateDataview(string name, string extentUri)
    {
        var viewExtent = _workspaceLogic.GetUserFormsExtent();
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
        _workspaceLogic.GetUserFormsExtent();

    public Workspace GetViewWorkspace() =>
        _workspaceLogic.GetViewsWorkspace();
}