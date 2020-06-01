﻿using System;
using DatenMeister.Models.DataViews;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
    public class DataViewPlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly DataViewLogic _dataViewLogic;

        /// <summary>
        /// Gets a list of types which need to be transferred as a MofType
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypes()
        {
            return new[]
            {
                typeof(DataView),
                typeof(ViewNode),
                typeof(SourceExtentNode),
                typeof(FlattenNode),
                typeof(FilterPropertyNode),
                typeof(FilterTypeNode),
                typeof(ComparisonMode),
                typeof(SelectPathNode)
            };
        }

        public DataViewPlugin(LocalTypeSupport localTypeSupport, IWorkspaceLogic workspaceLogic, DataViewLogic dataViewLogic)
        {
            _localTypeSupport = localTypeSupport;
            _workspaceLogic = workspaceLogic;
            _dataViewLogic = dataViewLogic;
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
                    var workspace = new Workspace(WorkspaceNames.WorkspaceViews, "Container of all views which are created dynamically.");
                    _workspaceLogic.AddWorkspace(workspace);
                    workspace.ExtentPlugins.Add(new DataViewExtentPlugin(_workspaceLogic, _dataViewLogic));
                    break;
                case PluginLoadingPosition.AfterLoadingOfExtents:
                    _localTypeSupport.ImportTypes(
                        DataViewLogic.PackagePathTypesDataView,
                        _DataViews.TheOne,
                        IntegrateDataViews.Assign
                    );
                    break;
            }
        }
    }
}