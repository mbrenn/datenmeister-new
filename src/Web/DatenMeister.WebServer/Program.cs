using System.Reflection;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.BootStrap.PublicSettings;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration.DotNet;
using DatenMeister.Plugins;
using DatenMeister.WebServer.Library.ServerConfiguration;
using LogLevel = BurnSystems.Logging.LogLevel;

namespace DatenMeister.WebServer;

public class Program
{
    private static volatile bool _performRestart;

    private static IHost? _host;

    /// <summary>
    /// Stops the program and performs a restart, if required
    /// </summary>
    /// <param name="restart">Flag, whether the application shall be restarted</param>
    public static async Task Stop(bool restart = false)
    {
        if (_host == null) return; // Nothing to do

        _performRestart = restart;
        await _host.StopAsync();
    }

    public static async Task Main(string[] args)
    {
        // Loads the Public Settings
        var publicSettingsPath = Assembly.GetEntryAssembly()?.Location;
        var configuration = PublicSettingHandler.LoadExtentFromDirectory(
            Path.GetDirectoryName(publicSettingsPath) 
            ?? throw new InvalidOperationException("Something obscure happened"), 
            out var path);

        // Initializes the Logging
        var settings = InitializeLogging(configuration);
        settings.SettingsFilePath = path;

        TheLog.Info("Welcome to DatenMeister");

        // Starts the webserver
        do
        {
            // Loads the DatenMeister
            var defaultSettings = GiveMe.GetDefaultIntegrationSettings();
            defaultSettings.IsLockingActivated = true;
            defaultSettings.AllowNoFailOfLoading = true;
            defaultSettings.AdditionalSettings.Add(
                new DefaultPluginSettings
                {
                    AssemblyFilesToBeSkipped =
                        new List<string>(new[] {"DatenMeister.WebServer.Views.dll"})
                });

            GiveMe.Scope = await GiveMe.DatenMeister(defaultSettings);

            // Loads the configuration for the webserver
            if (configuration != null)
            {
                var webserverConfig = WebServerSettingsLoader.LoadSettingsFromExtent(configuration);
                GiveMe.Scope.ScopeStorage.Add(webserverConfig);
                WebServerSettingHandler.TheOne.WebServerSettings = webserverConfig;
            }

            _performRestart = false;
            _host = CreateHostBuilder(args).Build();
            await _host.RunAsync();
        } while (_performRestart);

        // Unloads the DatenMeister
        await GiveMe.TryGetScope()!.UnuseDatenMeister();
        TheLog.Info("Good bye - Your DatenMeister");
    }

    private static PublicIntegrationSettings InitializeLogging(IExtent? configurationExtent)
    {
#if DEBUG
        TheLog.FilterThreshold = LogLevel.Trace;
        TheLog.AddProvider(new DebugProvider(), LogLevel.Trace);
        TheLog.AddProvider(InMemoryDatabaseProvider.TheOne, LogLevel.Trace);
#else
            TheLog.AddProvider(InMemoryDatabaseProvider.TheOne);
#endif
        // Preload Public Settings
        var publicSettings = 
            PublicSettingHandler.ParseSettingsFromFile(configurationExtent)
            ?? new PublicIntegrationSettings();
        if (publicSettings is not { LogLocation: LogLocation.None })
        {
            var location = publicSettings?.LogLocation ?? LogLocation.Application;

            var logPath = location switch
            {
                LogLocation.Application =>
                    Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location)!,
                        "log.txt"),
                LogLocation.Desktop =>
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "DatenMeister.log.txt"),
                LogLocation.LocalAppData =>
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "DatenMeister/log.txt"),
                _ => Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location)!, "log.txt")
            };

            TheLog.AddProvider(new FileProvider(logPath, true), LogLevel.Trace);
        }

        TheLog.AddProvider(new ConsoleProvider(), LogLevel.Trace);
        return publicSettings!;
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}