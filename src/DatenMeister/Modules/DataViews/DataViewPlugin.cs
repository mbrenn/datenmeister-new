﻿using System;
using DatenMeister.Core.Plugins;
using DatenMeister.Modules.DataViews.Model;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping | PluginLoadingPosition.AfterInitialization)]
    public class DataViewPlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Gets a list of types which need to be transferred as a MofType
        /// </summary>
        /// <returns></returns>
        public static Type[] GetTypes()
        {
            return new[]
            {
                typeof(DataView),
                typeof(ViewNode)
            };
        }

        public DataViewPlugin(LocalTypeSupport localTypeSupport, IWorkspaceLogic workspaceLogic)
        {
            _localTypeSupport = localTypeSupport;
            _workspaceLogic = workspaceLogic;
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
                    var workspace = new Workspace("Views", "Container of all views which are created dynamically.");
                    _workspaceLogic.AddWorkspace(workspace);
                    break;
                case PluginLoadingPosition.AfterInitialization:
                    _localTypeSupport.AddInternalTypes(GetTypes(), DataViewLogic.PackagePathTypesDataView);
                    break;
            }
        }
    }
}