using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            TheLog.AddProvider(new DebugProvider());
            TheLog.AddProvider(new ConsoleProvider());
            TheLog.AddProvider(new FileProvider("Stundenmeister.log", true));
        }
    }
}
