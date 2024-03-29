﻿#nullable enable

using System.Diagnostics;
using Autofac;
using DatenMeister.BootStrap;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces.Data;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Plugins;

namespace DatenMeister.Integration.DotNet
{
    /// <summary>
    /// Implements home helper classes
    /// </summary>
    public static class Helper
    {
        private static Integrator? _integration;

        public static IDatenMeisterScope UseDatenMeister(
            this ContainerBuilder kernel,
            IntegrationSettings settings,
            PluginLoaderSettings? pluginLoaderSettings = null)
        {
            pluginLoaderSettings ??= new PluginLoaderSettings();
            _integration = new Integrator(settings, pluginLoaderSettings);
            return _integration.UseDatenMeister(kernel);
        }

        /// <summary>
        /// Stores all data that needs to be stored persistent on the hard drive
        /// This method is typically called at the end of the lifecycle of the application
        /// </summary>
        /// <param name="scope">Kernel to be used to find the appropriate methods</param>
        public static void UnuseDatenMeister(this IDatenMeisterScope scope)
        {
            Debug.Assert(_integration != null, nameof(_integration) + " != null");
            _integration.UnuseDatenMeister();
            
            var integrationSettings = scope.ScopeStorage.Get<IntegrationSettings>();
            if (!integrationSettings.IsReadOnly)
            {
                scope.Resolve<WorkspaceLoader>().Store();
            }
            
            scope.Resolve<ExtentManager>().UnloadManager(true);
        }
    }
}