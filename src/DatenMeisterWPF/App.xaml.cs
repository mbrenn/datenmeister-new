using System;
using System.IO;
using System.Threading.Tasks;
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
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(App));
        
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            SetupExceptionHandling();
            var path = Path.Combine(Path.GetDirectoryName(typeof(App).Assembly.Location)!, "log.txt");

            TheLog.AddProvider(new DebugProvider(), LogLevel.Trace);
            TheLog.AddProvider(new FileProvider(path, true), LogLevel.Trace);
            TheLog.AddProvider(InMemoryDatabaseProvider.TheOne, LogLevel.Debug);
#if DEBUG
            TheLog.FilterThreshold = LogLevel.Trace;
#else
            TheLog.FilterThreshold = LogLevel.Info;
#endif
            TheLog.Info("Starting DatenMeister WPF");
        }
        
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            try
            {
                GiveMe.TryGetScope()?.UnuseDatenMeister();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }

            TheLog.Info("Exiting DatenMeister WPF");
        }
        
        /// <summary>
        /// Source: https://stackoverflow.com/questions/793100/globally-catch-exceptions-in-a-wpf-application
        /// </summary>
        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            try
            {
                System.Reflection.AssemblyName assemblyName =
                    System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
            }
            catch (Exception ex)
            {
                ClassLogger.Error(ex.ToString(), "Exception in LogUnhandledException");
            }
            finally
            {
                MessageBox.Show(
                    $"An exception occured: \r\n\r\n{exception.Message}\r\n\r\nMore detail about " +
                    $"the exception can be found in the log. " +
                    $"Save your data and restart application. It might go well... or not...");
                ClassLogger.Error(exception.ToString(), message);
            }
        }
    }
}
