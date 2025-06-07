using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Forms.Helper;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Forms.Actions;

internal class NavigateToFieldsForTestActionHandler : IActionHandler
{
    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        // First, create a temporary object
        var temporaryExtentLogic = new TemporaryExtentLogic(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
        var temporaryObject = temporaryExtentLogic.CreateTemporaryElement(null, TimeSpan.FromHours(1));

        // Second, navigate the user to the recently created object with the testing form
        var navigateToItem = InMemoryObject.CreateEmpty(_Actions.TheOne.ClientActions.__NavigateToItemClientAction);
        navigateToItem.set(_Actions._ClientActions._NavigateToItemClientAction.workspaceId, temporaryExtentLogic.WorkspaceName);
        navigateToItem.set(_Actions._ClientActions._NavigateToItemClientAction.itemUrl, temporaryObject.GetUri());
        navigateToItem.set(_Actions._ClientActions._NavigateToItemClientAction.formUri, Uris.TestFormUri);

        var result = InMemoryObject.CreateEmpty(_Actions.TheOne.__ActionResult);
        result.AddCollectionItem(_Actions._ActionResult.clientActions, navigateToItem);
        return await Task.FromResult<IElement?>(result);
    }

    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Forms.TheOne.__NavigateToFieldsForTestAction) == true;
    }
}