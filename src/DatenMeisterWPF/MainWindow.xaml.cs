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
    public partial class MainWindow : Window, INavigationHost
    {
        /// <summary>
        /// Stores the icon repository
        /// </summary>
        private IIconRepository _iconRepository;

        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly List<RibbonTab> _ribbonTabs = new List<RibbonTab>();

        private async void Window_Initialized(object sender, EventArgs e)
        {
            MainControl.Content = new IntroScreen();
            App.Scope = await Task.Run(
                () => GiveMe.DatenMeister());


            LoadIconRepository();

            Navigator.TheNavigator.NavigateToWorkspaces(this);
        }

        /// <summary>
        /// Loads the icon repository. 
        /// If DatenMeister.Icons is existing, then the full and cool icons will be used. 
        /// </summary>
        private void LoadIconRepository()
        {
            if (File.Exists("DatenMeister.Icons.dll"))
            {
                var dllPath = Path.Combine(Environment.CurrentDirectory, "DatenMeister.Icons.dll");
                var assembly = Assembly.LoadFile(dllPath);

                var type = assembly.GetType("DatenMeister.Icons.NiceIconsRepository");
                _iconRepository = Activator.CreateInstance(type) as IIconRepository;
            }

            if (_iconRepository == null)
            {
                _iconRepository = new StandardRepository();
            }
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
                ClearRibbons();
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

                PrepareDefaultNavigation();
                if (userControl is INavigationGuest guest)
                {
                    guest.PrepareNavigation();
                }

                FinalizeRibbons();

                return result;
            }

            return null;
        }

        /// <summary>
        /// Clears the complete MainRibbon navigaton
        /// </summary>
        private void ClearRibbons()
        {
            _ribbonTabs.Clear();
            MainRibbon.Items.Clear();

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
        }

        /// <summary>
        /// After having received the MainRibbon requests, this method builds up the real navigation
        /// </summary>
        private void FinalizeRibbons()
        {
            AddNavigationButton("About",
                () => new AboutDialog
                {
                    Owner = this
                }.ShowDialog(),
                "file-about",
                NavigationCategories.File);
        }

        /// <summary>
        /// Prepares the default navigation
        /// </summary>
        private void PrepareDefaultNavigation()
        {
            AddNavigationButton("Close", 
                Close,
                "file-exit",
                NavigationCategories.File);
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
            string tabName, groupName;
            var indexOfSemicolon = categoryName.IndexOf('.');
            if (indexOfSemicolon == -1)
            {
                tabName = categoryName;
                groupName = "Standard";
            }
            else
            {
                tabName = categoryName.Substring(0, indexOfSemicolon);
                groupName = categoryName.Substring(indexOfSemicolon + 1);
            }

            var tab = _ribbonTabs.FirstOrDefault(x => x.Header?.ToString() == tabName);
            if (tab == null)
            {
                tab = new RibbonTab
                {
                    Header = tabName
                };

                _ribbonTabs.Add(tab);
                MainRibbon.Items.Add(tab);
            }
            
            var group = tab.Items.OfType<RibbonGroup>().FirstOrDefault(x => x.Header.ToString() == groupName);
            if (group == null)
            {
                group = new RibbonGroup
                {
                    Header = groupName
                };
                tab.Items.Add(group);
            }

            var button = new RibbonButton
            {
                Label = name,
                LargeImageSource = _iconRepository.GetIcon(imageName)
            };

            button.Click += (x, y) => clickMethod();
            group.Items.Add(button);
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
    }
}
