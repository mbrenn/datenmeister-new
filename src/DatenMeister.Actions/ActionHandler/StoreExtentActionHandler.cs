using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Actions.ActionHandler;

public class StoreExtentActionHandler : IActionHandler
{
    /// <summary>
    /// Defines the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(StoreExtentActionHandler)); 
        
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__StoreExtentAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var workspaceName = action.getOrDefault<string>(_Actions._DropExtentAction.workspaceId) ??
                                "Data";
            var extentUri = action.getOrDefault<string>(_Actions._DropExtentAction.extentUri);

            var extentManager = new ExtentManager(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
            if (actionLogic.WorkspaceLogic.FindExtent(workspaceName, extentUri) is not MofExtent extent)
            {
                throw new InvalidOperationException(
                    $"Extent was not found: {workspaceName}-{extentUri}");
            }

            var result = extentManager.GetProviderLoaderAndConfiguration(workspaceName, extentUri);
            if (result.providerLoader == null || result.loadConfiguration == null)
            {
                throw new InvalidOperationException(
                    $"ProviderLoader was not found: {workspaceName}-{extentUri}");
            }

            result.providerLoader.StoreProvider(extent.Provider, result.loadConfiguration);

            Logger.Info(
                $"Extent stored manually: {workspaceName}-{extentUri}");
        });

        return null;
    }
}