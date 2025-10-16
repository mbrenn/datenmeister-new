using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView.Evaluation;
using DatenMeister.Plugins;

namespace DatenMeister.DataView;

[PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
public class DataViewPlugin(
    IWorkspaceLogic workspaceLogic,
    DataViewLogic dataViewLogic,
    IScopeStorage scopeStorage)
    : IDatenMeisterPlugin
{
    /// <summary>
    /// Starts the plugin
    /// </summary>
    /// <param name="position"></param>
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterBootstrapping:
                var workspace = new Workspace(WorkspaceNames.WorkspaceViews,
                    "Container of all views which are created dynamically.");
                workspace.IsDynamicWorkspace = true;
                workspaceLogic.AddWorkspace(workspace);
                workspace.ExtentFactory.Add(new DataViewExtentFactory(dataViewLogic, scopeStorage));

                scopeStorage.Get<ResolveHookContainer>().Add(new DataViewResolveHook());
                break;
            case PluginLoadingPosition.AfterLoadingOfExtents:
                var factories = GetDefaultViewNodeFactories();
                scopeStorage.Add(factories);
                break;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the default view node factories
    /// </summary>
    /// <returns>The found view node factories</returns>
    public static DataViewNodeFactories GetDefaultViewNodeFactories()
    {
        var result = new DataViewNodeFactories();
        result.Add(new DynamicSourceNodeEvaluation());            
        result.Add(new RowFilterByPropertyValueNodeEvaluation());
        result.Add(new RowFilterByMetaclassNodeEvaluation());
        result.Add(new RowFlattenNodeEvaluation());
        result.Add(new SelectByPathNodeEvaluation());
        result.Add(new SelectByFullNameNodeEvaluation());
        result.Add(new SelectByExtentNodeEvaluation());
        result.Add(new SelectFromAllWorkspacesNodeEvaluation());
        result.Add(new SelectByWorkspaceNodeEvaluation());
        result.Add(new ColumnFilterExcludeEvaluation());
        result.Add(new ColumnFilterIncludeOnlyEvaluation());
        result.Add(new RowFilterOnPositionEvaluation());
        result.Add(new RowFilterNodeEvaluation());
        result.Add(new RowFilterByFreeTextAnywhereNodeEvaluation());
        result.Add(new NodeReferenceNodeEvaluation());

        return result;
    }
}