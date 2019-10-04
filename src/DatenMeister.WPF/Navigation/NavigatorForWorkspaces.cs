using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Modules.ViewFinder.Helper;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Lists;
using Workspace = DatenMeister.Runtime.Workspaces.Workspace;

namespace DatenMeister.WPF.Navigation
{
    public static class NavigatorForWorkspaces
    {
        /// <summary>
        /// Navigates to the workspaces
        /// </summary>
        /// <param name="window">Windows to be used</param>
        /// <returns>The navigation to control the view</returns>
        public static Task<NavigateToElementDetailResult> NavigateToWorkspaces(INavigationHost window)
        {
            return window.NavigateTo(
                () => new WorkspaceList(),
                NavigationMode.List);
        }

        public static void OpenFolder(INavigationHost window)
        {
            var integrationSettings = GiveMe.Scope.Resolve<IntegrationSettings>();
            Process.Start(integrationSettings.DatabasePath);
        }

        /// <summary>
        /// Creates a new workspace by asking the user about new workspace information
        /// </summary>
        /// <param name="navigationHost"></param>
        /// <returns></returns>
        public static async Task<NavigateToElementDetailResult> CreateNewWorkspace(INavigationHost navigationHost)
        {
            var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
            var viewExtent = viewLogic.GetInternalViewExtent();

            var formElement = NamedElementMethods.GetByFullName(
                viewExtent,
                ManagementViewDefinitions.PathNewWorkspaceForm);
            if (formElement == null)
            {
                var creator = GiveMe.Scope.Resolve<FormCreator>();
                var managementProvider =  GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_ManagementProvider>();
                formElement = creator.CreateDetailFormByMetaClass(managementProvider.__CreateNewWorkspaceModel);
            }

            var result = await NavigatorForItems.NavigateToElementDetailViewAsync(
                navigationHost,
                new NavigateToItemConfig
                {
                    FormDefinition = formElement
                });

            if (result.Result == NavigationResult.Saved)
            {
                var workspaceId = result.DetailElement.get("id").ToString();
                var annotation = result.DetailElement.get("annotation").ToString();

                var workspace = new Workspace(workspaceId, annotation);
                var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                workspaceLogic.AddWorkspace(workspace);
            }

            return result;
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
            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var extentManager = GiveMe.Scope.Resolve<IExtentManager>();
            var integrationSettings = GiveMe.Scope.Resolve<IntegrationSettings>();
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

                    var extentStoragePath = loadConfiguration.filePath;
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

            GiveMe.Scope.UnuseDatenMeister();
            GiveMe.Scope = null;


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