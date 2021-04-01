using System;
using System.IO;
using System.Reflection;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.PublicSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DatenMeister.WebServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitializeLogging();

            // Loads the DatenMeister
            var defaultSettings = GiveMe.GetDefaultIntegrationSettings();
            defaultSettings.IsLockingActivated = true;

            GiveMe.Scope = GiveMe.DatenMeister(defaultSettings);
                
            // Starts the webserver
            CreateHostBuilder(args).Build().Run();
            
            // Unloads the Datenmeister
            GiveMe.TryGetScope()?.UnuseDatenMeister();
        }

        private static void InitializeLogging()
        {
            TheLog.AddProvider(new DebugProvider(), LogLevel.Trace);

            // Preload Public Settings
            var publicSettingsPath = Assembly.GetEntryAssembly()?.Location;
            var publicSettings =
                PublicSettingHandler.LoadSettingsFromDirectory(
                    Path.GetDirectoryName(publicSettingsPath) ?? throw new InvalidOperationException("Path returned null"));
            if (publicSettings == null || publicSettings.logLocation != LogLocation.None)
            {
                var location = publicSettings?.logLocation ?? LogLocation.Application;

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

            TheLog.AddProvider(InMemoryDatabaseProvider.TheOne, LogLevel.Debug);
            TheLog.AddProvider(new ConsoleProvider(), LogLevel.Debug);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
