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

        private IDatenMeisterScope _scope;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            MainControl.Content = new IntroScreen();
            _scope = await Task.Run(
                () => GiveMe.DatenMeister());
            MainControl.Content = null;
            
            var workspaceControl = new WorkspaceViewControl();
            workspaceControl.SetContent(_scope);
            MainControl.Content = workspaceControl;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _scope?.UnuseDatenMeister();
        }
    }
}
