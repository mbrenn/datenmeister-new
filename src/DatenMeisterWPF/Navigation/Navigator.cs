using System;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Lists;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Navigation
{
    public enum NavigationMode
    {
        List, 
        Detail,
        ForceNewWindow
    }

    /// <summary>
    /// Defines the possible categories for navigation
    /// </summary>
    public static class NavigationCategories
    {
        /// <summary>
        /// Category for all global and file issues
        /// </summary>
        public static string File = "File";
    }

    /// <summary>
    /// Defines the navigation method to allow a fluent navigation between instances
    /// </summary>
    public class Navigator
    {
        /// <summary>
        /// Stores the navigation for the given application
        /// </summary>
        public static Navigator TheNavigator { get; } = new Navigator();

        /// <summary>
        /// Performs a navigation from the current window to the given User Control.
        /// The user control is stored as an action method to allow
        /// </summary>
        /// <param name="root">Root window to be used</param>
        /// <param name="factoryMethod">The factory method</param>
        /// <param name="navigationMode">Defines the navigation mode</param>
        public IControlNavigation NavigateTo(
            INavigationHost root,
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode)
        {
            // Verifies if the given window supports the navigation. 
            if (root is INavigationHost navigateable)
            {
                var innerResult = navigateable.NavigateTo(factoryMethod, navigationMode);
                if (innerResult != null)
                {
                    return innerResult;
                }
            }

            var result = new ControlNavigation();
            var userControl = factoryMethod();
            switch (userControl)
            {
                case null:
                    return null;
                case ListViewControl asListViewControl:
                {
                    var window = new ListFormWindow
                    {
                        Owner = root as Window,
                        MainViewSet =
                        {
                            Content = asListViewControl
                        }
                    };

                    window.Closed += (x, y) => result.OnClosed();
                    break;
                }
                case DetailFormControl asDetailFormControl:
                {
                    var window = new DetailFormWindow
                    {
                        Owner = root as Window,
                        MainContent =
                        {
                            Content = asDetailFormControl
                        }
                    };

                    window.Show();
                    window.Closed += (x, y) => result.OnClosed();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Navigates to the workspaces
        /// </summary>
        /// <param name="window">Windows to be used</param>
        /// <returns>The navigation to control the view</returns>
        public IControlNavigation NavigateToWorkspaces(INavigationHost window)
        {
            return NavigateTo(
                window,
                () =>
                {

                    var viewLogic = App.Scope.Resolve<ViewLogic>();

                    var workspaceListView = NamedElementMethods.GetByFullName(
                        viewLogic.GetViewExtent(),
                        ViewDefinitions.PathWorkspaceListView);

                    var workspaceControl = new WorkspaceList();
                    workspaceControl.SetContent(workspaceListView);
                    return workspaceControl;
                },
                NavigationMode.List);
        }

        /// <summary>
        /// Navigates to an extent list
        /// </summary>
        /// <param name="window">Root window being used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns>The navigation being used to control the view</returns>
        public IControlNavigation NavigateToExtentList(INavigationHost window, string workspaceId)
        {
            return NavigateTo(
                window,
                () =>
                {
                    var dlg = new ExtentList();
                    dlg.SetContent(workspaceId);
                    return dlg;
                },
                NavigationMode.List);
        }

        /// <summary>
        /// Navigates to the detail window
        /// </summary>
        /// <param name="window">Window which is the owner for the detail window</param>
        /// <param name="element">Element to be shown</param>
        /// <returns>The navigation being used to control the view</returns>
        public IControlNavigation NavigateToElementDetailView(INavigationHost window, IElement element)
        {
            return NavigateTo(
                window, () =>
                {
                    var control = new DetailFormControl();
                    control.SetContent(element, null);
                    control.AllowNewProperties = true;
                    control.AddDefaultButtons();
                    return control;
                },
                NavigationMode.Detail);
        }

        /// <summary>
        /// Opens the dialog in which the user can create a new xmi extent
        /// </summary>
        /// <param name="window">Window being used as an owner</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns></returns>
        public IControlNavigation NavigateToNewXmiExtentDetailView(
            INavigationHost window,
            string workspaceId)
        {
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            return NavigateTo(
                window,
                () =>
                {
                    var newXmiDetailForm = NamedElementMethods.GetByFullName(
                        viewLogic.GetViewExtent(),
                        ViewDefinitions.PathNewXmiDetailForm);

                    var control = new DetailFormControl();
                    control.SetContent(null, newXmiDetailForm);
                    control.AddDefaultButtons("Create");
                    control.ElementSaved += (x, y) =>
                    {
                        var configuration = new XmiStorageConfiguration
                        {
                            ExtentUri = control.DetailElement.isSet("uri")
                                ? control.DetailElement.get("uri").ToString()
                                : string.Empty,
                            Path = control.DetailElement.isSet("filepath")
                                ? control.DetailElement.get("filepath").ToString()
                                : string.Empty,
                            Workspace = workspaceId
                        };

                        var extentManager = App.Scope.Resolve<IExtentManager>();
                        extentManager.LoadExtent(configuration, true);
                    };

                    return control;
                },
                NavigationMode.Detail);

        }

        public IControlNavigation NavigateToItemsInExtent(
            INavigationHost window,
            string workspaceId,
            string extentUrl)
        {
            return NavigateTo(window, () =>
            {
                var control = new ItemsInExtentList();
                control.SetContent(App.Scope, workspaceId, extentUrl);

                return control;
            },
            NavigationMode.List);
        }

        public IControlNavigation SearchByUrl(INavigationHost window)
        {
            var dlg = new QueryElementDialog {Owner = window as Window};
            if (dlg.ShowDialog() == true)
            {
                var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
                var element = workspaceLogic.FindItem(dlg.QueryUrl.Text);

                if (element == null)
                {
                    MessageBox.Show("Item does not exist.");
                    return null;
                }

                return NavigateToElementDetailView(
                    window,
                    element);
            }

            return null;
        }
    }
}