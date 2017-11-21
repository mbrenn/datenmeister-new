using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using DatenMeister.Integration;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INavigationHost, IHasRibbon
    {
        private readonly RibbonHelper RibbonHelper;

        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        public Ribbon GetRibbon() => MainRibbon;

        public MainWindow()
        {
            InitializeComponent();
            RibbonHelper = new RibbonHelper(this);
        }


        private async void Window_Initialized(object sender, EventArgs e)
        {
            MainControl.Content = new IntroScreen();
            App.Scope = await Task.Run(
                () => GiveMe.DatenMeister());

            RibbonHelper.LoadIconRepository();

            Navigator.TheNavigator.NavigateToWorkspaces(this);
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
                RibbonHelper.ClearRibbons();

                RibbonHelper.AddNavigationButton(
                    "Workspaces",
                    () => Navigator.TheNavigator.NavigateToWorkspaces(this),
                    Icons.WorkspacesShow,
                    NavigationCategories.File + ".Workspaces");

                RibbonHelper.AddNavigationButton(
                    "Search",
                    () => Navigator.TheNavigator.SearchByUrl(this),
                    null,
                    NavigationCategories.File + ".Search");

                var result = new ControlNavigation();
                var userControl = factoryMethod();

                MainControl.Content = userControl;

                switch (userControl)
                {
                    case DetailFormControl detailForm:
                        detailForm.NavigationHost = this;
                        break;
                    case ListViewControl listForm:
                        listForm.NavigationHost = this;
                        break;
                }

                RibbonHelper.PrepareDefaultNavigation();
                if (userControl is INavigationGuest guest)
                {
                    guest.PrepareNavigation();
                }

                RibbonHelper.FinalizeRibbons();

                return result;
            }

            return null;
        }
        
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
            RibbonHelper.AddNavigationButton(name, clickMethod, imageName, categoryName);
        }
    }
}
