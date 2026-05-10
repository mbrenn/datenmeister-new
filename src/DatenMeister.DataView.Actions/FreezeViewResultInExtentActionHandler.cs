using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.DataView.Actions;

public class FreezeViewResultInExtentHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Data.__FreezeViewResultInExtent) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var extentLoaderConfig = action.getOrDefault<IElement?>(_Actions._Data._FreezeViewResultInExtent.extentLoaderConfig);
        
        if(extentLoaderConfig == null)
        {
            throw new InvalidOperationException("extentLoaderConfig is null");
        }
        
        return await FreezeViewResultInMemoryHandler.TransferElementsFromViewNodeToExtent(
            actionLogic.WorkspaceLogic,
            actionLogic.ScopeStorage,
            extentLoaderConfig,
            action);
    }
}