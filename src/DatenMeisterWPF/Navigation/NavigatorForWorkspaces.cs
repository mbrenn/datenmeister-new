using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Lists;

namespace DatenMeisterWPF.Navigation
{
    public static class NavigatorForWorkspaces
    {
        /// <summary>
        /// Navigates to the workspaces
        /// </summary>
        /// <param name="window">Windows to be used</param>
        /// <returns>The navigation to control the view</returns>
        public static IControlNavigation NavigateToWorkspaces(INavigationHost window)
        {
            return window.NavigateTo(
                () =>
                {
                    var workspaceControl = new WorkspaceList {IsTreeVisible = true};
                    return workspaceControl;
                },
                NavigationMode.List);
        }

        public static void OpenFolder(INavigationHost window)
        {
            var integrationSettings = App.Scope.Resolve<IntegrationSettings>();
            Process.Start(integrationSettings.DatabasePath);
        }

        /// <summary>
        /// Resets the complete DatenMeister application... Will lead to a restart of the application 
        /// </summary>
        /// <param name="navigationHost">Navigation host to be used</param>
        public static void ResetDatenMeister(INavigationHost navigationHost)
        {
            if (MessageBox.Show(
                    "Are you sure, that you would like to reset the DatenMeister. All types, all views and all extents will be removed. The storage files for extents in Data Workspace will not be affected.\r\n\r\n" +
                    "After deleting all information, the DatenMeister will be restarted.",
                    "Reset DatenMeister?", MessageBoxButton.YesNo)
                != MessageBoxResult.Yes)
            {
                return;
            }

            // Ok... what to do now? 
            // Collect all files... 
            var files = new List<string>();
            var workspaceLogic= App.Scope.Resolve<IWorkspaceLogic>();
            var extentManager = App.Scope.Resolve<IExtentManager>();
            var integrationSettings = App.Scope.Resolve<IntegrationSettings>();
            foreach (var workspace in workspaceLogic.Workspaces)
            {
                if (workspace.id == WorkspaceNames.NameData)
                {
                    continue;
                }

                foreach (var extent in workspaceLogic.GetExtentsForWorkspace(workspace))
                {
                    var loadConfiguration = extentManager.GetLoadConfigurationFor(extent) as ExtentFileLoaderConfig;
                    if (loadConfiguration == null)
                    {
                        continue;
                    }

                    var extentStoragePath = loadConfiguration.Path;
                    files.Add(extentStoragePath);
                }
            }

            // Unload DatenMeister
            navigationHost.GetWindow().Close();
            if (navigationHost.GetWindow().IsActive)
            {
                MessageBox.Show("DatenMeister was not closed");
                return;
            }
            
            App.Scope.UnuseDatenMeister();
            App.Scope = null;


            foreach (var file in files)
            {
                File.Delete(file);
            }

            File.Delete(Integrator.GetPathToWorkspaces(integrationSettings));
            File.Delete(Integrator.GetPathToExtents(integrationSettings));

            // Restarts the DatenMeister
            var location = Assembly.GetEntryAssembly().Location;
            if (Path.GetExtension(location).EndsWith("exe"))
            {
                Process.Start(Assembly.GetEntryAssembly().Location);
            }
            else
            {
                MessageBox.Show("The DatenMeister was not started by the .exe... So restart is not possible.");
            }
        }
    }
}