using BurnSystems.CommandLine;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Action.Executor;
using DatenMeister.Integration.DotNet;
using DatenMeister.Plugins;

Console.WriteLine("Hello, World!");

var arguments = Parser.ParseIntoOrShowUsage<Arguments>(args);
if (arguments == null)
{
    return;
}

// Activate Logging
#if DEBUG
    TheLog.FilterThreshold = LogLevel.Trace;
    TheLog.AddProvider(new DebugProvider(), LogLevel.Trace);
    TheLog.AddProvider(InMemoryDatabaseProvider.TheOne, LogLevel.Trace);
#else
    TheLog.AddProvider(InMemoryDatabaseProvider.TheOne);
#endif

// Loads the DatenMeister
var defaultSettings = GiveMe.GetDefaultIntegrationSettings();
defaultSettings.IsLockingActivated = true;
defaultSettings.AllowNoFailOfLoading = true;
defaultSettings.IsReadOnly = true;

try
{
    GiveMe.Scope = await GiveMe.DatenMeister(defaultSettings);
}
finally
{
    GiveMe.Scope.Dispose();
}