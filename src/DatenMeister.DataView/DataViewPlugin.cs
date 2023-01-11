using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView.Evaluation;
using DatenMeister.Plugins;

namespace DatenMeister.DataView
{
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    public class DataViewPlugin : IDatenMeisterPlugin
    {
        private readonly DataViewLogic _dataViewLogic;
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public DataViewPlugin(
            IWorkspaceLogic workspaceLogic,
            DataViewLogic dataViewLogic,
            IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _dataViewLogic = dataViewLogic;
            _scopeStorage = scopeStorage;
        }

        /// <summary>
        /// Starts the plugin
        /// </summary>
        /// <param name="position"></param>
        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    var workspace = new Workspace(WorkspaceNames.WorkspaceViews,
                        "Container of all views which are created dynamically.");
                    workspace.IsDynamicWorkspace = true;
                    _workspaceLogic.AddWorkspace(workspace);
                    workspace.ExtentFactory.Add(new DataViewExtentFactory(_dataViewLogic, _scopeStorage));

                    _scopeStorage.Get<ResolveHookContainer>().Add(new DataViewResolveHook());
                    break;
                case PluginLoadingPosition.AfterLoadingOfExtents:
                    var factories = GetDefaultViewNodeFactories();
                    _scopeStorage.Add(factories);
                    break;
            }
        }

        /// <summary>
        /// Gets the default view node factories
        /// </summary>
        /// <returns>The found view node factories</returns>
        public static DataViewNodeFactories GetDefaultViewNodeFactories()
        {
            var result = new DataViewNodeFactories();
            result.Add(new DynamicSourceNodeEvaluation());
            result.Add(new FilterPropertyNodeEvaluation());
            result.Add(new FilterTypeNodeEvaluation());
            result.Add(new FlattenNodeEvaluation());
            result.Add(new SelectByFullNameNodeEvaluation());
            result.Add(new SourceExtentNodeEvaluation());

            return result;
        }
    }
}