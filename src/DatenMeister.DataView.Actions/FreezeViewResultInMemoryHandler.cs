using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.DataView.Actions;

public class FreezeViewResultInMemoryHandler: IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Data.__FreezeViewResultInMemory) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var workspaceLogic = actionLogic.WorkspaceLogic;
        var scopeStorage = actionLogic.ScopeStorage;
        
        var extentUri = action.getOrDefault<string>(_Actions._Data._FreezeViewResultInMemory.extentUri);

        if (string.IsNullOrEmpty(extentUri))
        {
            throw new InvalidOperationException("extentUri is null or empty");
        }

        var extentLoaderConfig = new ExtentLoaderConfigs.InMemoryLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
        {
            extentUri = extentUri
        }.GetWrappedElement();

        return await TransferElementsFromViewNodeToExtent(workspaceLogic, scopeStorage, extentLoaderConfig, action);
    }

    /// <summary>
    /// Transfers the elements from the view node to the extent
    /// </summary>
    /// <param name="workspaceLogic">Workspace logic to be used for accessing the workspace</param>
    /// <param name="scopeStorage">Scope storage to be used for accessing the scope</param>
    /// <param name="extentLoaderConfig">Extent loader configuration to be used for loading the extent</param>
    /// <param name="action">Action element containing the viewnode</param>
    /// <returns>null</returns>
    public static async Task<IElement?> TransferElementsFromViewNodeToExtent(IWorkspaceLogic workspaceLogic,
        IScopeStorage scopeStorage, IElement extentLoaderConfig, IElement action)
    {
        var viewNode = action.getOrDefault<IElement>(_Actions._Data._FreezeViewResultInMemory.viewNode);
        if (viewNode == null)
        {
            throw new InvalidOperationException("viewNode is null");
        }

        // Get the elements of the view
        var dataViewEvaluation = new DataViewEvaluation(workspaceLogic, scopeStorage);
        var elements = dataViewEvaluation.GetElementsForViewNode(viewNode);
     
        // Creates the extent
        var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
        var extentResult = await extentManager.LoadExtent(extentLoaderConfig, ExtentCreationFlags.CreateOnly);
        var extent = extentResult.Extent;
        if (extent == null ||
            extentResult.LoadingState is not (ExtentLoadingState.Loaded or ExtentLoadingState.LoadedReadOnly))
        {
            throw new InvalidOperationException("Could not load the extent: " + extentResult.FailLoadingMessage);
        }

        var factory = new MofFactory(extent);
        
        // Now add the events to the extent
        foreach (var element in elements.OfType<IElement>())
        {
            extent.elements().add(ObjectCopier.Copy(
                factory, element, CopyOptions.None));
        }
        
        return null;
    }
}