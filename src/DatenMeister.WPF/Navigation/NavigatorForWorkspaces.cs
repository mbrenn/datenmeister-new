﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.BootStrap;
using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Forms.FormCreator;
using DatenMeister.Integration.DotNet;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Helper;

namespace DatenMeister.WPF.Navigation
{
    public static class NavigatorForWorkspaces
    {
        /// <summary>
        /// Navigates to the workspaces
        /// </summary>
        /// <param name="window">Windows to be used</param>
        /// <returns>The navigation to control the view</returns>
        public static Task<NavigateToElementDetailResult?> NavigateToWorkspaces(INavigationHost window)
        {
            return window.NavigateTo(
                () => new WorkspaceList(),
                NavigationMode.List);
        }

        public static void OpenFolder(INavigationHost window)
        {
            var integrationSettings = GiveMe.Scope.ScopeStorage.Get<IntegrationSettings>();
            DotNetHelper.CreateProcess(integrationSettings.DatabasePath);
        }

        /// <summary>
        /// Creates a new workspace by asking the user about new workspace information
        /// </summary>
        /// <param name="navigationHost"></param>
        /// <returns></returns>
        public static async Task<NavigateToElementDetailResult?>? CreateNewWorkspace(INavigationHost navigationHost)
        {
            var viewLogic = GiveMe.Scope.Resolve<FormMethods>();
            var viewExtent = viewLogic.GetInternalFormExtent();

            var formElement = NamedElementMethods.GetByFullName(
                viewExtent,
                ManagementViewDefinitions.PathNewWorkspaceForm);
            if (formElement == null)
            {
                var creator = GiveMe.Scope.Resolve<FormCreator>();
                formElement =
                    creator.CreateRowFormByMetaClass(_DatenMeister.TheOne.Management.__CreateNewWorkspaceModel);
            }

            var result = await NavigatorForItems.NavigateToElementDetailView(
                navigationHost,
                new NavigateToItemConfig
                {
                    Form = new FormDefinition(formElement)
                });

            if (result != null && result.Result == NavigationResult.Saved)
            {
                var detailElement =
                    result.DetailElement ?? throw new InvalidOperationException("DetailElement == null");

                var workspaceId = detailElement.getOrDefault<string>("id");
                var annotation = detailElement.getOrDefault<string>("annotation");

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
            var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
            var integrationSettings = GiveMe.Scope.ScopeStorage.Get<IntegrationSettings>();
            foreach (var workspace in workspaceLogic.Workspaces)
            {
                if (workspace.id == WorkspaceNames.WorkspaceData)
                {
                    continue;
                }

                foreach (var extent in workspaceLogic.GetExtentsForWorkspace(workspace))
                {
                    var loadConfiguration = extentManager.GetLoadConfigurationFor(extent);
                    if (loadConfiguration != null)
                    {
                        var extentStoragePath =
                            loadConfiguration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs
                                ._ExtentFileLoaderConfig.filePath);

                        if (extentStoragePath != null)
                        {
                            files.Add(extentStoragePath);
                        }
                    }
                }
            }

            // Unload DatenMeister
            if (navigationHost.GetWindow() is IApplicationWindow mainWindow)
            {
                mainWindow.DoCloseWithoutAcknowledgement = true;
            }

            Application.Current.Exit += (x, y) =>
            {
                if (navigationHost.GetWindow().IsActive)
                {
                    MessageBox.Show("DatenMeister was not closed");
                    return;
                }

                GiveMe.Scope.UnuseDatenMeister();
                GiveMe.Scope = null!;

                foreach (var file in files)
                {
                    File.Delete(file);
                }

                File.Delete(Integrator.GetPathToWorkspaces(integrationSettings));
                File.Delete(Integrator.GetPathToExtents(integrationSettings));

                // Restarts the DatenMeister
                var entryAssembly = Assembly.GetEntryAssembly() ??
                                    throw new InvalidOperationException("Assembly.GetEntryAssembly is null");
                var location = entryAssembly.Location;
                if (Path.GetExtension(location).EndsWith("exe"))
                {
                    DotNetHelper.CreateProcess(entryAssembly.Location);
                }
                else
                {
                    MessageBox.Show("The DatenMeister was not started by the .exe... So restart is not possible.");
                }
            };

            navigationHost.GetWindow().Close();
        }
    }
}