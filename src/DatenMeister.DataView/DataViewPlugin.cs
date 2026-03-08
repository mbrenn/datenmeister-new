using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView.Evaluation;
using DatenMeister.Plugins;

namespace DatenMeister.DataView;

/// <summary>
/// Implements the plugin for the data views
/// </summary>
/// <param name="workspaceLogic">The workspace logic</param>
/// <param name="dataViewLogic">The logic for the data view</param>
/// <param name="scopeStorage">The scope storage</param>
[PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
public class DataViewPlugin(
    IWorkspaceLogic workspaceLogic,
    DataViewLogic dataViewLogic,
    IScopeStorage scopeStorage)
    : IDatenMeisterPlugin
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private ILogger logger = new ClassLogger(typeof(DataViewPlugin));

    /// <summary>
    /// Starts the plugin
    /// </summary>
    /// <param name="position">The position at which the plugin is started</param>
    /// <returns>A task representing the operation</returns>
    public Task Start(PluginLoadingPosition position)
    {
        var managementWorkspace = workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceManagement);
        
        switch (position)
        {
            case PluginLoadingPosition.AfterBootstrapping:
                if (managementWorkspace == null)
                {
                    Logger.Warn("Management workspace not found. Automated registering will not be performed");
                }

                var workspace = new Workspace(WorkspaceNames.WorkspaceViews,
                    "Container of all views which are created dynamically.")
                {
                    IsDynamicWorkspace = true
                };
                
                workspaceLogic.AddWorkspace(workspace);
                workspace.ExtentFactory.Add(new DataViewExtentFactory(dataViewLogic, scopeStorage));

                scopeStorage.Get<DataViewCache>().MarkAsDirty();
                scopeStorage.Get<ResolveHookContainer>().Add(new DataViewResolveHook());


                break;
            case PluginLoadingPosition.AfterLoadingOfExtents:
                var factories = GetDefaultViewNodeFactories();
                scopeStorage.Add(factories);

                // Adds the change management to identify changes in Management Workspace. 
                // After a change is notified, the views are marked as dirty
                if (managementWorkspace != null)
                {
                    workspaceLogic.ChangeEventManager.RegisterFor(
                        managementWorkspace,
                        (w, e, o) => Task.Run(() =>
                        {
                            scopeStorage.Get<DataViewCache>().MarkAsDirty();
                        }));
                }

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
        result.Add(new RowOrderByNodeEvaluation());
        result.Add(new RowFilterByFreeTextAnywhereNodeEvaluation());
        result.Add(new NodeReferenceNodeEvaluation());

        return result;
    }
}