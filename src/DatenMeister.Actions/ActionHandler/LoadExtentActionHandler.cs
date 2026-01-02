using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Actions.ActionHandler;

public class LoadExtentActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__LoadExtentAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        return await Task.Run(async () =>
        {
            var configuration =
                action.getOrDefault<IElement>(_Actions._LoadExtentAction.configuration);
            if (configuration == null)
            {
                throw new InvalidOperationException("configuration is null");
            }

            var extentUri =
                configuration.getOrDefault<string>(_ExtentLoaderConfigs._ExtentLoaderConfig
                    .extentUri);
            if (string.IsNullOrEmpty(extentUri))
            {
                throw new InvalidOperationException("extentUri is null");
            }

            var workspaceId =
                configuration.getOrDefault<string>(
                    _ExtentLoaderConfigs._ExtentLoaderConfig.workspaceId);
            if (string.IsNullOrEmpty(workspaceId))
            {
                workspaceId = WorkspaceNames.WorkspaceData;
            }

            var dropExisting =
                action.getOrDefault<bool>(_Actions._LoadExtentAction.dropExisting);

            if (configuration == null)
            {
                throw new InvalidOperationException("No configuration is set");
            }

            var extentManager = new ExtentManager(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
            if (dropExisting)
            {
                await extentManager.RemoveExtent(workspaceId, extentUri);
            }

            var result = await extentManager.LoadExtent(
                configuration,
                ExtentCreationFlags.LoadOrCreate);

            if (result.LoadingState == ExtentLoadingState.Failed)
            {
                throw new InvalidOperationException(
                    "Loading of extent failed:\r\n\r\n" + result.FailLoadingMessage);
            }

            var resultingElement = InMemoryObject.CreateEmpty(_Actions.TheOne.ParameterTypes.__LoadExtentActionResult.Uri);
            resultingElement.set(_Actions._ParameterTypes._LoadExtentActionResult.workspaceId, workspaceId);
            resultingElement.set(_Actions._ParameterTypes._LoadExtentActionResult.extentUri, extentUri);
            return resultingElement;
        });
    }
}