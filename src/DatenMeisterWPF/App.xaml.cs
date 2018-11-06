using System;
using System.Windows;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Integration;

namespace DatenMeisterWPF
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets or sets the scope for the DatenMeister
        /// </summary>
        public static IDatenMeisterScope Scope { get; set; }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            try
            {
                Scope?.UnuseDatenMeister();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }

            TheLog.Info("Exiting DatenMeister WPF");
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            TheLog.AddProvider(new DebugProvider(), LogLevel.Trace);
            TheLog.Info("Starting DatenMeister WPF");
        }
    }
}
