using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Forms.Model;

namespace DatenMeister.Extent.Forms.MassImport;

internal class PerformMassImportActionHandler(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    : IActionHandler
{
    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        // Check the extent
        var extent = action.getOrDefault<IObject>(_Root._MassImportDefinitionAction.item);
        var text = action.getOrDefault<string>(_Root._MassImportDefinitionAction.text);

        // Assumes that the given extent is a definition of Extent-Management instance

        var workspace = extent.getOrDefault<string>(_DatenMeister._Management._Extent.workspaceId);
        var extentUri = extent.getOrDefault<string>(_DatenMeister._Management._Extent.uri);

        var foundExtent = workspaceLogic.FindExtent(workspace, extentUri);

        if (foundExtent == null)
        {
            throw new InvalidOperationException("The given 'item' is not an extent.");
        }

        // We got all the data together, so let's do the massimport
        var massImportLogic = new MassImportLogic(workspaceLogic, scopeStorage);
        massImportLogic.PerformMassImport(foundExtent, text);

        // Now return the client action
        var result = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ActionResult);
        var clientAction = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.ClientActions.__NavigateToExtentClientAction);
        result.AddCollectionItem(_DatenMeister._Actions._ActionResult.clientActions, clientAction);
        clientAction.set(_DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.workspaceId, workspace);
        clientAction.set(_DatenMeister._Actions._ClientActions._NavigateToExtentClientAction.extentUri, extentUri);

        return await Task.FromResult(result);
    }

    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Root.TheOne.__MassImportDefinitionAction) == true;
    }
}