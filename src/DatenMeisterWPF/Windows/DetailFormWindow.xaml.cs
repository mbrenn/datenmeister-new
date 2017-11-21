using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für DetailFormWindow.xaml
    /// </summary>
    public partial class DetailFormWindow : Window, IHasRibbon, INavigationHost
    {
        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        public Ribbon GetRibbon() => MainRibbon;

        private RibbonHelper RibbonHelper { get; set; }

        public DetailFormWindow()
        {
            InitializeComponent();
            RibbonHelper = new RibbonHelper(this);
        }

        private void DetailFormWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            RibbonHelper.ClearRibbons();

            AddNavigationButton(
                "Workspaces",
                () => Navigator.TheNavigator.NavigateToWorkspaces(this),
                Icons.WorkspacesShow,
                NavigationCategories.File + ".Workspaces");

            AddNavigationButton(
                "Search",
                () => Navigator.TheNavigator.SearchByUrl(this),
                null,
                NavigationCategories.File + ".Search");
            
            RibbonHelper.PrepareDefaultNavigation();
            RibbonHelper.FinalizeRibbons();
        }

        public IControlNavigation NavigateTo(Func<UserControl> factoryMethod, NavigationMode navigationMode)
        {
            throw new NotImplementedException();
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

        public void SetFocus()
        {
            Focus();
        }
    }
}
