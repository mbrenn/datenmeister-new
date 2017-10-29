using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms;
using DatenMeisterWPF.Forms.Lists;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INavigationHost
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            MainControl.Content = new IntroScreen();
            App.Scope = await Task.Run(
                () => GiveMe.DatenMeister());
            var viewLogic = App.Scope.Resolve<ViewLogic>();

            var workspaceListView = NamedElementMethods.GetByFullName(
                viewLogic.GetViewExtent(),
                ViewDefinitions.PathWorkspaceListView);

            var workspaceControl = new WorkspaceList();
            workspaceControl.SetContent(App.Scope, workspaceListView);
            MainControl.Content = workspaceControl;
        }

        private List<RibbonTab> _ribbonTabs = new List<RibbonTab>();

        public IControlNavigation NavigateTo(
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode)
        {
            _ribbonTabs.Clear();

            if (navigationMode == NavigationMode.List)
            {
                var result = new ControlNavigation();
                var userControl = factoryMethod();
                if (userControl is INavigationGuest guest)
                {
                    guest.PrepareNavigation(this);
                }

                MainControl.Content = userControl;
                return result;
            }


            return null;
        }

        public void AddNavigationElement(string name, Action clickMethod, string imageName, string categoryName)
        {
            var tab = new RibbonTab()
            {
                Header = categoryName
            };

            ribbon.Items.Add(tab);
        }
    }
}
