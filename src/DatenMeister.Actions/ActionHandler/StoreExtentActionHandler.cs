using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Actions.ActionHandler
{
    public class StoreExtentActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__StoreExtentAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspaceName = action.getOrDefault<string>(_DatenMeister._Actions._DropExtentAction.workspace) ?? "Data";
            var extentUri = action.getOrDefault<string>(_DatenMeister._Actions._DropExtentAction.extentUri);

            var extentManager = new ExtentManager(actionLogic.WorkspaceLogic, actionLogic.ScopeStorage);
            var extent = actionLogic.WorkspaceLogic.FindExtent(workspaceName, extentUri) as MofExtent;
            if (extent == null)
            {
                throw new InvalidOperationException("Extent was not found: " + workspaceName + "-" + extent);
            }

            var result = extentManager.GetProviderLoaderAndConfiguration(workspaceName, extentUri);
            if (result.providerLoader == null || result.loadConfiguration == null)
            {
                throw new InvalidOperationException("ProviderLoader was not found" + workspaceName + "-" + extent);
            }

            result.providerLoader.StoreProvider(extent.Provider, result.loadConfiguration);
        }
    }
}