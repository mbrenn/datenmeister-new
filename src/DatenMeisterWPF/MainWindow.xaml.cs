using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using Autofac;
using BurnSystems;
using DatenMeister.Integration;
using DatenMeister.NetCore;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Locking;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeisterWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INavigationHost, IHasRibbon, IApplicationWindow
    {
        private readonly RibbonHelper _ribbonHelper;

        /// <inheritdoc />
        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        public Ribbon GetRibbon() => MainRibbon;

        /// <summary>
        /// Initializes a new instance of the MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _ribbonHelper = new RibbonHelper(
                this,
                NavigationScope.Application |
                NavigationScope.Extent |
                NavigationScope.Item);
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            MainControl.Content = new IntroScreen();
            var defaultSettings = GiveMeDotNetCore.GetDefaultIntegrationSettings();
            defaultSettings.IsLockingActivated = true;

            try
            {
                GiveMe.Scope = await Task.Run(
                    () => GiveMeDotNetCore.DatenMeister(defaultSettings));
            }
            catch (IsLockedException exception)
            {
                MessageBox.Show($"The following file is currently locked:\r\n\r\n{exception.FilePath}\r\n\r\n" +
                                $"The Application will be closed. Please check whether another application is running.");
                DoCloseWithoutAcknowledgement = true;
                Close();
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }

            _ribbonHelper.LoadIconRepository();

            //NavigatorForWorkspaces.NavigateToWorkspaces(this);
            _ = NavigatorForExtents.NavigateToExtentList(this, WorkspaceNames.WorkspaceData);

            var integrationSettings = GiveMe.Scope.ScopeStorage.Get<IntegrationSettings>();
            var extentStorageData = GiveMe.Scope.ScopeStorage.Get<ExtentStorageData>();
            if (extentStorageData.FailedLoading)
            {
                Title += " (READ-ONLY)";
                var message = extentStorageData.FailedLoadingException == null
                    ? "No message"
                    : extentStorageData.FailedLoadingException.ToString();

                var failedExtents = extentStorageData.FailedLoadingExtents
                    .Select(x=>$"- {x}")
                    .Join("\r\n");

                if (MessageBox.Show("An exception occured during the loading of the events. \r\n" +
                                    "This will lead to a read-only instance of DatenMeister. All changes will be lost. \r\n" +
                                    "To resolve the issue, verify the log and fix the 'DatenMeister.Extents.xml'.\n\n" +
                                    "Would you like to open the corresponding folder?\r\n\r\nThe affected extents are:\r\n" +
                                    failedExtents + "\r\n\r\nThe message that occured lastly:\r\n" +
                                    message,
                        "Error during start-up of DatenMeister",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var databasePath = integrationSettings.DatabasePath;

                    DotNetHelper.CreateProcess(databasePath);
                }
            }
            
            // Sets the title of the mainwindow
            if (integrationSettings.WindowTitle != null)
            {
                Title = integrationSettings.WindowTitle;
            }
        }

        /// <summary>
        /// Called, if the host shall navigate to a certain control
        /// </summary>
        /// <param name="factoryMethod">Method being used to create the control</param>
        /// <param name="navigationMode">Navigation mode defining whether to create a new window or something similar</param>
        /// <returns>The navigation instance supporting events and other methods. </returns>
        public Task<NavigateToElementDetailResult> NavigateTo(
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

                // Unregisters the currently created element
                if (MainControl.Content is ICanUnregister canUnregister)
                {
                    canUnregister.Unregister();
                }

                MainControl.Content = userControl;

                var task = new TaskCompletionSource<NavigateToElementDetailResult>();
                task.SetResult(new NavigateToElementDetailResult());
                return task.Task;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Rebuild the complete navigation by first collecting the view extensions and
        /// then by putting it into the ribbon helper
        /// </summary>
        public void RebuildNavigation()
        {
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;

            // 1) The default properties of the main window
            var viewExtensions = new List<ViewExtension>
            {
                new ApplicationMenuButtonDefinition(
                    "Goto Data",
                    () => NavigatorForExtents.NavigateToExtentList(this, WorkspaceNames.WorkspaceData),
                    Icons.FileHome,
                    NavigationCategories.DatenMeisterNavigation,
                    10),
                new ApplicationMenuButtonDefinition(
                    "Goto Workspaces",
                    () => NavigatorForWorkspaces.NavigateToWorkspaces(this),
                    Icons.WorkspacesShow,
                    NavigationCategories.DatenMeisterNavigation,
                    9),
                new ApplicationMenuButtonDefinition(
                    "Find by URL",
                    () => NavigatorForDialogs.SearchByUrl(this),
                    null,
                    NavigationCategories.DatenMeister + ".Database"),
                new ApplicationMenuButtonDefinition(
                    "Locate",
                    () => NavigatorForDialogs.LocateAndOpen(this),
                    null,
                    NavigationCategories.DatenMeister + ".Database"),
                new ApplicationMenuButtonDefinition(
                    "Open Log",
                    OpenLog,
                    null,
                    NavigationCategories.DatenMeister + ".Tool"),
                new ApplicationMenuButtonDefinition(
                    "Close",
                    Close,
                    "file-exit",
                    NavigationCategories.DatenMeister + ".Tool"),
                new ApplicationMenuButtonDefinition("About",
                    () => new AboutDialog
                    {
                        Owner = this
                    }.ShowDialog(),
                    "file-about",
                    NavigationCategories.DatenMeister + ".Tool")
            };

            // 2) The properties of the guest
            var guest = MainControl.Content as INavigationGuest;
            if (guest != null)
            {
                var guestViewExtensions = guest.GetViewExtensions().ToList();
                viewExtensions = viewExtensions.Union(guestViewExtensions).ToList();
            }

            // 3) The plugins
            var data = new ViewExtensionInfoApplication(this, guest);

            foreach (var plugin in viewExtensionPlugins)
            {
                viewExtensions.AddRange(plugin.GetViewExtensions(data));
            }

            // Now execute them on the ribbon itself
            _ribbonHelper.Item = MainControl.Content is IItemNavigationGuest itemNavigationGuest ? itemNavigationGuest.Item : null;
            _ribbonHelper.Collection = MainControl.Content is ICollectionNavigationGuest collectionNavigationGuest ? collectionNavigationGuest.Collection : null;
            _ribbonHelper.Extent = MainControl.Content is IExtentNavigationGuest extentNavigationGuest ? extentNavigationGuest.Extent : null;
            _ribbonHelper.EvaluateExtensions(viewExtensions);

            // And on the guest
            guest?.EvaluateViewExtensions(viewExtensions);

            void OpenLog()
            {
                var wnd = new LogWindow {Owner = this};
                wnd.Show();
            }
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

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Unregisters the currently created element
            if (MainControl.Content is ICanUnregister canUnregister)
            {
                canUnregister.Unregister();
            }
            
            
            // Asks the user, if he was not already asked before

            if (!DoCloseWithoutAcknowledgement)
            {
                var integrationSettings = GiveMe.Scope.ScopeStorage.Get<IntegrationSettings>();
                var windowTitle = integrationSettings.WindowTitle ?? "Der DatenMeister";
                if (MessageBox.Show(
                    $"Are you sure, that you would like to close '{windowTitle}'",
                    $"Close {windowTitle}?",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <inheritdoc />
        public Window GetWindow() => this;

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public bool DoCloseWithoutAcknowledgement { get; set; }
    }
}
