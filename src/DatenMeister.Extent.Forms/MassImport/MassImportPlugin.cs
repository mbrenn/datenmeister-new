using DatenMeister.Actions;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core;
using DatenMeister.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Extent.Forms.MassImport
{
    [PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
    internal class MassImportPlugin : IDatenMeisterPlugin
    {
        private readonly IWorkspaceLogic workspaceLogic;
        private readonly IScopeStorage scopeStorage;

        public MassImportPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            this.workspaceLogic = workspaceLogic;
            this.scopeStorage = scopeStorage;
        }

        public Task Start(PluginLoadingPosition position)
        {
            scopeStorage.Get<ActionLogicState>().AddActionHandler(
                new PerformMassImportActionHandler(workspaceLogic, scopeStorage));

            return Task.CompletedTask;
        }
    }
}
