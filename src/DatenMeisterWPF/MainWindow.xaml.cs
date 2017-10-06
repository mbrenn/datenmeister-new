using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
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
            MainControl.Content = null;

            var viewLogic = App.Scope.Resolve<ViewLogic>();

            var workspaceListView = NamedElementMethods.GetByFullName(
                viewLogic.GetViewExtent(),
                "Management::WorkspaceListView");

            var workspaceControl = new WorkspaceList();
            workspaceControl.SetContent(App.Scope, workspaceListView);
            MainControl.Content = workspaceControl;
        }
    }
}
