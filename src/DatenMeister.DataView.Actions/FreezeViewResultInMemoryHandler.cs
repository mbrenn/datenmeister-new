using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.DataView.Actions;

public class FreezeViewResultInMemoryHandler: IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Data.__FreezeViewResultInExtent) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var workspaceLogic = actionLogic.WorkspaceLogic;
        var scopeStorage = actionLogic.ScopeStorage;
        
        var extentUri = action.getOrDefault<string>(_Actions._Data._FreezeViewResultInMemory.extentUri);
        var viewNode = action.getOrDefault<IElement>(_Actions._Data._FreezeViewResultInMemory.viewNode);

        if (string.IsNullOrEmpty(extentUri))
        {
            throw new InvalidOperationException("extentUri is null or empty");
        }

        if (viewNode == null)
        {
            throw new InvalidOperationException("viewNode is null");
        }

        // Get the elements of the view
        var dataViewEvaluation = new DataViewEvaluation(workspaceLogic, scopeStorage);
        var elements = dataViewEvaluation.GetElementsForViewNode(viewNode);
     
        // Creates the extent
        var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
        var extentResult = await extentManager.LoadExtent(
            new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
            {
                extentUri = extentUri
            }.GetWrappedElement());
        var extent = extentResult.Extent;
        if (extent == null ||
            extentResult.LoadingState is not (ExtentLoadingState.Loaded or ExtentLoadingState.LoadedReadOnly))
        {
            throw new InvalidOperationException("Could not load the extent: " + extentResult.FailLoadingMessage);
        }
        
        // Now add the events to the extent
        foreach (var element in elements.OfType<IElement>())
        {
            extent.elements().add(element);
        }
        return null;
    }
}