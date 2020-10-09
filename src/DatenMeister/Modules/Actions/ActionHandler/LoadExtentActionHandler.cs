using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class LoadExtentActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__LoadExtentAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var configuration =
                action.getOrDefault<IElement>(_DatenMeister._Actions._LoadExtentAction.configuration);
            var extentUri =
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig.extentUri);
            var workspaceId =
                configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig.workspaceId)
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
                extentManager.RemoveExtent(workspaceId, extentUri);
            }
            
            extentManager.LoadExtent(
                configuration, 
                ExtentCreationFlags.LoadOrCreate);
        }
    }
}