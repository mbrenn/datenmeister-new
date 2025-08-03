namespace StundenMeister;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Stores the path in which the data files and log files are stored
    /// </summary>
    public static string StorageFilePath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "StundenMeister");

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        TheLog.FilterThreshold = LogLevel.Debug;
        TheLog.AddProvider(new DebugProvider());
        TheLog.AddProvider(new ConsoleProvider());
        TheLog.AddProvider(new FileProvider(
            Path.Combine(StorageFilePath, "Stundenmeister.log"),
            true));

        AppDomain.CurrentDomain.UnhandledException += (x, y) => { TheLog.Fatal("Exception: " + y.ExceptionObject); };
        Current.DispatcherUnhandledException += (x,y) => { TheLog.Fatal("Exception: " + y.Exception); };
        TaskScheduler.UnobservedTaskException += (x,y) => { TheLog.Fatal("Exception: " + y.Exception); };
    }


    private void App_OnExit(object sender, ExitEventArgs e)
    {
    }
}