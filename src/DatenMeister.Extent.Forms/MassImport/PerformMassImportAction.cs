using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Forms.Model;
using System;
using System.Threading.Tasks;

namespace DatenMeister.Extent.Forms.MassImport
{
    internal class PerformMassImportActionHandler : IActionHandler
    {
        private readonly IWorkspaceLogic workspaceLogic;
        private readonly IScopeStorage scopeStorage;

        public PerformMassImportActionHandler(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            this.workspaceLogic = workspaceLogic;
            this.scopeStorage = scopeStorage;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            // Check the extent
            var extent = action.getOrDefault<IObject>(_Root._MassImportDefinitionAction.item);
            var text = action.getOrDefault<string>(_Root._MassImportDefinitionAction.text);

            var workspace = extent.getOrDefault<string>(_DatenMeister._Management._Extent.workspaceId);
            var extentUri = extent.getOrDefault<string>(_DatenMeister._Management._Extent.uri);

            var foundExtent = workspaceLogic.FindExtent(workspace, extentUri);

            if (foundExtent == null)
            {
                throw new InvalidOperationException("The given 'item' is not an extent.");
            }

            // Now return the client action
            var result = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ActionResult);
            var clientAction = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.ClientActions.__AlertClientAction);
            result.AddCollectionItem(_DatenMeister._Actions._ActionResult.clientActions, clientAction);
            clientAction.set(_DatenMeister._Actions._ClientActions._AlertClientAction.messageText, "This is a test");

            return await Task.FromResult(result);
        }

        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _Root.TheOne.__MassImportDefinitionAction) == true;
        }
    }
}
