﻿using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Plugins;

namespace DatenMeister.Provider.ExtentManagement
{
    /// <summary>
    ///     Defines a plugin for the management provider
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class ManagementProviderPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        ///     Extent of workspaces
        /// </summary>
        public const string UriExtentWorkspaces = WorkspaceNames.UriExtentWorkspaces;

        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentSettings _extentSettings;

        public const string WorkspaceExtentType = "DatenMeister.Workspaces";

        public ManagementProviderPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        public void Start(PluginLoadingPosition position)
        {
            // Adds the extent setting
            _extentSettings.extentTypeSettings.Add(new ExtentType(WorkspaceExtentType));
            
            // Adds the extent
            var managementExtent = new MofUriExtent(
                new ExtentOfWorkspaceProvider(_workspaceLogic, _scopeStorage),
                WorkspaceNames.UriExtentWorkspaces, _scopeStorage);
            
            managementExtent.ExtentConfiguration.ExtentType = WorkspaceExtentType;
            
            // Adds the extent containing the workspaces
            _workspaceLogic.GetManagementWorkspace().AddExtent(managementExtent);
        }

        /// <summary>
        ///     Gets the extent that contains the workspaces
        /// </summary>
        /// <param name="workspaceLogic">Logic for the workspace to be used</param>
        /// <returns>The found uri extent</returns>
        public static IUriExtent GetExtentForWorkspaces(IWorkspaceLogic workspaceLogic)
        {
            return workspaceLogic.GetManagementWorkspace().FindExtent(WorkspaceNames.UriExtentWorkspaces)
                   ?? throw new InvalidOperationException("Extent for uri extents not found");
        }
    }
}