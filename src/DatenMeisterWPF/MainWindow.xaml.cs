﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Modules;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

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
            GiveMe.Scope = await Task.Run(
                () => GiveMe.DatenMeister());

            _ribbonHelper.LoadIconRepository();

            //NavigatorForWorkspaces.NavigateToWorkspaces(this);
            _ = NavigatorForExtents.NavigateToExtentList(this, WorkspaceNames.NameData);

            if (GiveMe.Scope.Resolve<ExtentStorageData>().FailedLoading)
            {
                Title += " (READ-ONLY)";

                if (MessageBox.Show("An exception occured during the loading of the events. \r\n" +
                                    "This will lead to a read-only instance of DatenMeister. All changes will be lost. \r\n" +
                                    "To resolve the issue, verify the log and fix the 'DatenMeister.Extents.xml'.\n\n" +
                                    "Would you like to open the corresponding folder?",
                        "Error during start-up of DatenMeister",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var databasePath = GiveMe.Scope.Resolve<IntegrationSettings>().DatabasePath;
                    Process.Start(databasePath);
                }
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
            // return Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);
        }

        /// <summary>
        /// Rebuild the complete navigation by first collecting the view extensions and
        /// then by putting it into the ribbon helper
        /// </summary>
        public void RebuildNavigation()
        {
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;

            var viewExtensions = new List<ViewExtension>
            {
                new ApplicationMenuButtonDefinition(
                    "Goto Home",
                    () => NavigatorForExtents.NavigateToExtentList(this, WorkspaceNames.NameData),
                    Icons.FileHome,
                    NavigationCategories.File + ".Workspaces",
                    10),
                new ApplicationMenuButtonDefinition(
                    "Goto Workspaces",
                    () => NavigatorForWorkspaces.NavigateToWorkspaces(this),
                    Icons.WorkspacesShow,
                    NavigationCategories.File + ".Workspaces",
                    9),
                new ApplicationMenuButtonDefinition(
                    "Find by URL",
                    () => NavigatorForDialogs.SearchByUrl(this),
                    null,
                    NavigationCategories.File + ".Search"),
                new ApplicationMenuButtonDefinition(
                    "Locate",
                    () => NavigatorForDialogs.LocateAndOpen(this),
                    null,
                    NavigationCategories.File + ".Search"),
                new ApplicationMenuButtonDefinition(
                    "Open Log",
                    OpenLog,
                    null,
                    NavigationCategories.File + ".Search"),                
                new ApplicationMenuButtonDefinition(
                    "Close",
                    Close,
                    "file-exit",
                    NavigationCategories.File),
                new ApplicationMenuButtonDefinition("About",
                    () => new AboutDialog
                    {
                        Owner = this
                    }.ShowDialog(),
                    "file-about",
                    NavigationCategories.File)
            };

            if (MainControl.Content is INavigationGuest guest)
            {
                var guestViewExtensions = guest.GetViewExtensions().ToList();
                foreach (var viewExtension in guestViewExtensions.OfType<RibbonButtonDefinition>())
                {
                    //viewExtension.FixTopCategoryIfNotFixed("Extent");
                }

                viewExtensions = viewExtensions.Union(guestViewExtensions).ToList();
            }

            /*
             * Gets the plugins for the MainWindow itself
             */
            var data = new ViewExtensionTargetInformation(ViewExtensionContext.Application)
            {
                NavigationHost = this
            };

            foreach (var plugin in viewExtensionPlugins)
            {
                foreach (var extension in plugin.GetViewExtensions(data))
                {
                    if (extension is RibbonButtonDefinition ribbonButtonDefinition)
                    {
                     //   ribbonButtonDefinition.FixTopCategoryIfNotFixed("Item");
                    }

                    viewExtensions.Add(extension);
                }
            }

            _ribbonHelper.EvaluateExtensions(viewExtensions);

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
