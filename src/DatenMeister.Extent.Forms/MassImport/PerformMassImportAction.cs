using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
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

        var workspace = extent.getOrDefault<string>(_Management._Extent.workspaceId);
        var extentUri = extent.getOrDefault<string>(_Management._Extent.uri);

        var foundExtent = workspaceLogic.FindExtent(workspace, extentUri);

        if (foundExtent == null)
        {
            throw new InvalidOperationException("The given 'item' is not an extent.");
        }

        // We got all the data together, so let's do the massimport
        var massImportLogic = new MassImportLogic(workspaceLogic, scopeStorage);
        massImportLogic.PerformMassImport(foundExtent, text);

        // Now return the client action
        var result = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionResult);
        var clientAction = InMemoryObject.CreateEmpty(_Actions.TheOne.ClientActions.__NavigateToExtentClientAction);
        result.AddCollectionItem(_Actions._ActionResult.clientActions, clientAction);
        clientAction.set(_Actions._ClientActions._NavigateToExtentClientAction.workspaceId, workspace);
        clientAction.set(_Actions._ClientActions._NavigateToExtentClientAction.extentUri, extentUri);

        return await Task.FromResult(result);
    }

    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Root.TheOne.__MassImportDefinitionAction) == true;
    }
}