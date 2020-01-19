using System;
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
        public static Task<NavigateToElementDetailResult>? SearchByUrl(INavigationHost window)
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

                return NavigatorForItems.NavigateToElementDetailView(
                    window,
                    element);
            }

            return null;
        }

        /// <summary>
        /// Lets the user decide for an item and if he selects it, it will be opened
        /// </summary>
        /// <param name="navigationHost">The navigation host to be used</param>
        public static void LocateAndOpen(INavigationHost navigationHost)
        {
            var dlg = new LocateItemDialog
            {
                WorkspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>(),
                AsToolBox = true,
                Owner = navigationHost as Window
            };


            dlg.ItemChosen += (x, y) =>
            {
                var item = y.Item;
                if (item == null) return;
                NavigatorForItems.NavigateToElementDetailView(
                    navigationHost,
                    item);
            };

            dlg.Show();
        }

        /// <summary>
        /// Locates a certain item by setting the workspace and extent uri
        /// </summary>
        /// <param name="navigationHost">Navigation host to be used</param>
        /// <param name="workspaceName">Name of the workspace which shall be pre-selected</param>
        /// <param name="extentUri">Uri of the workspace </param>
        /// <returns>The selected workspace by the user or null, if the user has not selected a workspace</returns>
        public static IObject Locate(INavigationHost navigationHost, string workspaceName, string extentUri)
        {
            var workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspace(workspaceName);
            IExtent extent = null;
            if (workspace != null)
            {
                extent = workspace.FindExtent(extentUri);
            }

            return Locate(navigationHost, workspace, extent);
        }

        /// <summary>
        /// Locates a certain item
        /// </summary>
        /// <param name="navigationHost">Navigation host to be used</param>
        /// <param name="workspace">Defines the workspace to which shall be navigated</param>
        /// <param name="defaultExtent">Extent that shall be opened per default</param>
        /// <returns></returns>
        public static IObject? Locate(INavigationHost navigationHost, IWorkspace? workspace = null, IExtent? defaultExtent = null)
        {
            var dlg = new LocateItemDialog
            {
                WorkspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>(),
                Owner = navigationHost.GetWindow(),
                SelectButtonText = "Select"
            };

            if (workspace != null && defaultExtent != null)
            {
                dlg.Select(defaultExtent);
            }
            else
            {
                if (workspace != null)
                {
                    dlg.Select(workspace);
                }

                if (defaultExtent != null)
                {
                    dlg.Select(defaultExtent);
                }
            }

            if (dlg.ShowDialog() == true)
            {
                return dlg.SelectedElement;
            }

            return null;
        }
    }
}