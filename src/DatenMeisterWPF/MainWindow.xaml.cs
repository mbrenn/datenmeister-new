using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INavigationHost, IHasRibbon
    {
        private readonly RibbonHelper _ribbonHelper;

        /// <inheritdoc />
        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        public Ribbon GetRibbon() => MainRibbon;

        public MainWindow()
        {
            InitializeComponent();
            _ribbonHelper = new RibbonHelper(this);
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            MainControl.Content = new IntroScreen();
            App.Scope = await Task.Run(
                () => GiveMe.DatenMeister());

            _ribbonHelper.LoadIconRepository();

            NavigatorForWorkspaces.NavigateToWorkspaces(this);
            //NavigatorForExtents.NavigateToExtentList(this, WorkspaceNames.NameData);
        }

        /// <summary>
        /// Called, if the host shall navigate to a certain control
        /// </summary>
        /// <param name="factoryMethod">Method being used to create the control</param>
        /// <param name="navigationMode">Navigation mode defining whether to create a new window or something similar</param>
        /// <returns>The navigation instance supporting events and other methods. </returns>
        public IControlNavigation NavigateTo(
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode)
        {
            // Only if navigation method is a list
            if (navigationMode == NavigationMode.List)
            {
                var userControl = factoryMethod();
                if (userControl is INavigationGuest navigationGuest)
                {
                    navigationGuest.NavigationHost = this;
                }

                MainControl.Content = userControl;

                var result = new ControlNavigation();
                return result;
            }

            return Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);
        }

        /// <summary>
        /// Rebuild the complete navigation
        /// </summary>
        public void RebuildNavigation()
        {

            IEnumerable<ViewExtension> viewExtensions = new List<ViewExtension>
            {
                new RibbonButtonDefinition(
                    "Home",
                    () => NavigatorForExtents.NavigateToExtentList(this, WorkspaceNames.NameData),
                    Icons.FileHome,
                    NavigationCategories.File + ".Workspaces"),
                new RibbonButtonDefinition(
                    "Workspaces",
                    () => NavigatorForWorkspaces.NavigateToWorkspaces(this),
                    Icons.WorkspacesShow,
                    NavigationCategories.File + ".Workspaces"),
                new RibbonButtonDefinition(
                    "Find by URL",
                    () => NavigatorForDialogs.SearchByUrl(this),
                    null,
                    NavigationCategories.File + ".Search"),
                new RibbonButtonDefinition(
                    "Locate",
                    () => NavigatorForDialogs.LocateAndOpen(this),
                    null,
                    NavigationCategories.File + ".Search")
            };

            viewExtensions = viewExtensions.Union(_ribbonHelper.GetDefaultNavigation());
            if (MainControl.Content is INavigationGuest guest)
            {
                guest.NavigationHost = this;
                viewExtensions = viewExtensions.Union(guest.GetViewExtensions());
            }

            _ribbonHelper.EvaluateExtensions(viewExtensions);
        }
        
        /// <summary>
        /// Sets the focus
        /// </summary>
        public void SetFocus()
        {
            Activate();
            Focus();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Adds a navigational element to the ribbons
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="clickMethod">Method, that shall be called, when the user clicks on the item</param>
        /// <param name="imageName">Name of the image being allocated</param>
        /// <param name="categoryName">Category of the MainRibbon to be added</param>
        public void AddNavigationButton(string name, Action clickMethod, string imageName, string categoryName)
        {
            _ribbonHelper.AddNavigationButton(name, clickMethod, imageName, categoryName);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show(
                    "Are you sure, that you would like to close Der DatenMeister",
                    "Close DatenMeister?",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        /// <inheritdoc />
        public Window GetWindow()
        {
            return this;
        }
    }
}
