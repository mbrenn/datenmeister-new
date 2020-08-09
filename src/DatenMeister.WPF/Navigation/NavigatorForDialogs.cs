using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Navigation
{
    public static class NavigatorForDialogs
    {
        /// <summary>
        /// Opens the window in which the user can search for an item by a specific url
        /// </summary>
        /// <param name="window">Navigation host being used to open up the new dialog</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static async Task<NavigateToElementDetailResult?> SearchByUrl(INavigationHost window)
        {
            var dlg = new QueryElementDialog {Owner = window as Window};
            if (dlg.ShowDialog() == true)
            {
                var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                var element = workspaceLogic.FindItem(dlg.QueryUrl.Text);

                if (element == null)
                {
                    MessageBox.Show("Item does not exist.");
                    return null;
                }

                return await NavigatorForItems.NavigateToElementDetailView(
                    window,
                    element);
            }

            return null;
        }

        /// <summary>
        /// Lets the user decide for an item and if he selects it, it will be opened
        /// </summary>
        /// <param name="navigationHost">The navigation host to be used</param>
        public static async Task<IObject?> LocateAndOpen(INavigationHost navigationHost)
        {
            var task = new TaskCompletionSource<IObject?>();
            
            var dlg = new LocateItemDialog
            {
                WorkspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>(),
                Owner = navigationHost as Window
            };

            if (dlg.ShowDialog() == true)
            {
                if (dlg.SelectedElement != null)
                {
                    task.SetResult(dlg.SelectedElement);
                    
                    await NavigatorForItems.NavigateToElementDetailView(
                        navigationHost,
                        dlg.SelectedElement);
                }
                else
                {
                    task.SetResult(null);    
                }
            }

            return await task.Task;
        }

        /// <summary>
        /// Locates a certain item by setting the workspace and extent uri
        /// </summary>
        /// <param name="navigationHost">Navigation host to be used</param>
        /// <param name="workspaceName">Name of the workspace which shall be pre-selected</param>
        /// <param name="extentUri">Uri of the workspace </param>
        /// <returns>The selected workspace by the user or null, if the user has not selected a workspace</returns>
        public static async Task<IObject?> Locate(INavigationHost navigationHost, string workspaceName, string extentUri)
        {
            var workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspace(workspaceName);
            IExtent? extent = null;
            if (workspace != null)
            {
                extent = workspace.FindExtent(extentUri);
            }

            return await Locate(navigationHost,
                new NavigatorForDialogConfiguration
                {
                    DefaultWorkspace = workspace,
                    DefaultExtent = extent
                });
        }

        /// <summary>
        /// Defines the configuration for the navigator for dialog
        /// </summary>
        public class NavigatorForDialogConfiguration
        {
            /// <summary>
            /// Gets or sets the title of the window
            /// </summary>
            public string? Title { get; set; }

            /// <summary>
            /// Gets or sets a description to indicate further information to the user
            /// </summary>
            public string? Description { get; set; }

            /// <summary>
            /// Gets or sets the button to be pressed when the dialog result is the selected item
            /// </summary>
            public string? OkButtonText { get; set; }
            
            /// <summary>
            /// Gets or sets the default workspace
            /// </summary>
            public IWorkspace? DefaultWorkspace { get; set; }
            
            /// <summary>
            /// Gets or sets the default extent
            /// </summary>
            public IExtent? DefaultExtent { get; set; }

            /// <summary>
            /// Defines the metaclasses to which the shall be filtered 
            /// </summary>
            public List<IElement> FilteredMetaClasses { get; set; } = new List<IElement>();
        }

        /// <summary>
        /// Locates a certain item
        /// </summary>
        /// <param name="navigationHost">Navigation host to be used</param>
        /// <param name="workspace">Defines the workspace to which shall be navigated</param>
        /// <param name="defaultExtent">Extent that shall be opened per default</param>
        /// <returns></returns>
        [Obsolete]
        public static async Task<IObject?> Locate(
            INavigationHost navigationHost,
            IWorkspace? workspace = null,
            IExtent? defaultExtent = null)
        {
            return await Locate(
                navigationHost,
                new NavigatorForDialogConfiguration
                {
                    DefaultWorkspace = workspace,
                    DefaultExtent = defaultExtent
                });
        }
        
        /// <summary>
        /// Opens a dialog and asks the user to locate an item 
        /// </summary>
        /// <param name="navigationHost">Navigation host to be used</param>
        /// <param name="configuration">Configuration to be used</param>
        /// <returns></returns>
        public static async Task<IObject?> Locate(
            INavigationHost navigationHost,
            NavigatorForDialogConfiguration configuration)
        {
            var task = new TaskCompletionSource<IObject?>();
            var dlg = new LocateItemDialog
            {
                WorkspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>(),
                Owner = navigationHost.GetWindow(),
                SelectButtonText = configuration.OkButtonText ?? "Select",
                Title = configuration.Title ?? "Locate Item",
                MessageText = configuration.Title ?? "Locate Item"
            };

            if (configuration.Description != null)
            {
                dlg.DescriptionText = configuration.Description;
            }

            if (configuration.FilteredMetaClasses?.Any() == true)
            {
                dlg.SetMetaClassesForFilter(configuration.FilteredMetaClasses);
            }

            if (configuration.DefaultWorkspace != null && configuration.DefaultExtent != null)
            {
                dlg.Select(configuration.DefaultExtent);
            }
            else
            {
                if (configuration.DefaultWorkspace != null)
                {
                    dlg.Select(configuration.DefaultWorkspace);
                }

                if (configuration.DefaultExtent != null)
                {
                    dlg.Select(configuration.DefaultExtent);
                }
            }

            if (dlg.ShowDialog() == true)
            {
                task.SetResult(dlg.SelectedElement);
                return await task.Task;
            }
            
            task.SetResult(null);
            return await task.Task;
        }
    }
}