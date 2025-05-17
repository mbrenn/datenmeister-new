using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Actions.ActionHandler
{
    public class LoadExtentActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__LoadExtentAction) == true;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(async () =>
            {
                var configuration =
                    action.getOrDefault<IElement>(_DatenMeister._Actions._LoadExtentAction.configuration);
                if (configuration == null)
                {
                    throw new InvalidOperationException("configuration is null");
                }

                var extentUri =
                    configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig
                        .extentUri);
                var workspaceId =
                    configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig
                        .workspaceId)
                    ?? WorkspaceNames.WorkspaceData;

                var dropExisting =
                    action.getOrDefault<bool>(_DatenMeister._Actions._LoadExtentAction.dropExisting);

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
            });

            return null;
        }
    }
}