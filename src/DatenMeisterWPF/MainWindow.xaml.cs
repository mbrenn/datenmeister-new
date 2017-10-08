using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms;
using DatenMeisterWPF.Forms.Lists;

namespace DatenMeisterWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
    }
}
