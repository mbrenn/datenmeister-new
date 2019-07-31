﻿using System;
using System.IO;
using System.Windows;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;

namespace StundenMeister
{
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
            TheLog.AddProvider(new DebugProvider());
            TheLog.AddProvider(new ConsoleProvider());
            TheLog.AddProvider(new FileProvider(
                Path.Combine(StorageFilePath, "Stundenmeister.log"),
                true));
        }


        private void App_OnExit(object sender, ExitEventArgs e)
        {
        }
    }
}
