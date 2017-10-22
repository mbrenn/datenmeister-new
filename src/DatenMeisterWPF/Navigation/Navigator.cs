using System;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Lists;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Navigation
{
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
        public IControlNavigation NavigateTo(
            Window root,
            Func<UserControl> factoryMethod)
        {
            var result = new ControlNavigation();
            var userControl = factoryMethod();
            switch (userControl)
            {
                case null:
                    return null;
                case ListViewControl asListViewControl:
                {
                        /*
                    var window = new ListFormWindow
                    {
                        Owner = root,
                        MainViewSet =
                        {
                            Content = asListViewControl
                        }
                    };
                    */
                    var mainWindow = (MainWindow) root;
                    mainWindow.MainControl.Content = asListViewControl;
                    mainWindow.Closed += (x, y) => result.OnClosed();
                    break;
                }
                case DetailFormControl asDetailFormControl:
                {
                    var window = new DetailFormWindow
                    {
                        Owner = root,
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
        /// Navigates to an extent list
        /// </summary>
        /// <param name="window">Root window being used</param>
        /// <param name="scope">Scope to be used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns>The navigation being used to control the view</returns>
        public IControlNavigation NavigateToExtentList(Window window, IDatenMeisterScope scope, string workspaceId)
        {
            return NavigateTo(
                window,
                () =>
                {
                    var dlg = new ExtentList();
                    dlg.SetContent(scope, workspaceId);
                    return dlg;
                });
        }

        /// <summary>
        /// Navigates to the detail window
        /// </summary>
        /// <param name="window">Window which is the owner for the detail window</param>
        /// <param name="scope">Scope to be used</param>
        /// <param name="element">Element to be shown</param>
        /// <returns>The navigation being used to control the view</returns>
        public IControlNavigation NavigateToElementDetailView(Window window, IDatenMeisterScope scope, IElement element)
        {
            return NavigateTo(
                window, () =>
                {
                    var control = new DetailFormControl();
                    control.SetContent(scope, element, null);
                    control.AllowNewProperties = true;
                    control.AddDefaultButtons();
                    return control;
                });
        }

        /// <summary>
        /// Opens the dialog in which the user can create a new xmi extent
        /// </summary>
        /// <param name="window">Window being used as an owner</param>
        /// <param name="scope">Scope of the Datenmeister</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns></returns>
        public IControlNavigation NavigateToNewXmiExtentDetailView(
            Window window, 
            IDatenMeisterScope scope,
            string workspaceId)
        {
            var viewLogic = scope.Resolve<ViewLogic>();
            return NavigateTo(
                window,
                () =>
                {
                    var newXmiDetailForm = NamedElementMethods.GetByFullName(
                        viewLogic.GetViewExtent(),
                        ViewDefinitions.PathNewXmiDetailForm);

                    var control = new DetailFormControl();
                    control.SetContent(scope, null, newXmiDetailForm);
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

                        var extentManager = scope.Resolve<IExtentManager>();
                        extentManager.LoadExtent(configuration, true);
                    };

                    return control;
                });

        }

        public IControlNavigation NavigateToItemsInExtent(
            Window window,
            IDatenMeisterScope scope,
            string workspaceId,
            string extentUrl)
        {
            return NavigateTo(window, () =>
            {
                var control = new ItemsInExtentList();
                control.SetContent(scope, workspaceId, extentUrl);

                return control;
            });
        }
    }
}