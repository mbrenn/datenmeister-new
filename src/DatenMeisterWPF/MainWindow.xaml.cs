using System;
using System.Threading.Tasks;
using System.Windows;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
using DatenMeisterWPF.Forms;

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
            var datenMeister = await Task.Run(
                () => GiveMe.DatenMeister());
            MainControl.Content = null;

            /*var workspaceControl = new WorkspaceViewControl();
            workspaceControl.Show(datenMeister);
            MainControl.Content = workspaceControl;*/

            
            var workspaceControl = new ElementListViewControl();
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(datenMeister);
            workspaceControl.Show(datenMeister, workspaceExtent.elements(), null);
            MainControl.Content = workspaceControl;
        }
    }
}
