using Autofac;
using BurnSystems.CommandLine;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Action.Executor;
using DatenMeister.Actions;
using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;

// Activate Logging
#if DEBUG
TheLog.FilterThreshold = LogLevel.Trace;
TheLog.AddProvider(new DebugProvider(), LogLevel.Trace);
TheLog.AddProvider(new ConsoleProvider(), LogLevel.Trace);
TheLog.AddProvider(new FileProvider(
    Path.Combine(IntegrationSettings.DefaultDatabasePath, "executor.log"), true));
#else
TheLog.AddProvider(new ConsoleProvider(), LogLevel.Info);
#endif

var arguments = Parser.ParseIntoOrShowUsage<Arguments>(args);
if (arguments == null)
{
    return;
}

// Convert the file argument into an absolute path of current directory

// Loads the DatenMeister
var defaultSettings = GiveMe.GetDefaultIntegrationSettings();
var databasePath = defaultSettings.DatabasePath = Path.Combine(defaultSettings.DatabasePath, Guid.NewGuid().ToString());
defaultSettings.IsLockingActivated = true;
defaultSettings.AllowNoFailOfLoading = true;
defaultSettings.IsReadOnly = true;
defaultSettings.NormalizeToCurrentDirectory = true;
ExtentConfigurationLoader.BreakOnFailedWorkspaceLoading = false;

try
{
    TheLog.Info("Welcome to the Executor of DatenMeister");

    GiveMe.Scope = await GiveMe.DatenMeister(defaultSettings);

    TheLog.Info("DatenMeister loaded successfully");

    // Loads the extent
    var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
    var loadConfig = new ExtentLoaderConfigs.XmiStorageLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
    {
        extentUri = "dm:///actions",
        filePath = arguments.XmiFileName
    };

    var extentLoadInfo =
        await extentManager.LoadExtent(loadConfig.GetWrappedElement(), ExtentCreationFlags.LoadOnly);
    if (extentLoadInfo.LoadingState is not (ExtentLoadingState.Loaded or ExtentLoadingState.LoadedReadOnly))
    {
        TheLog.Error($"Extent could not be loaded: {extentLoadInfo.FailLoadingMessage}");
        return;
    }

    var actionExtent = extentLoadInfo.Extent ?? throw new InvalidOperationException("Extent is null");
    var actionElement = actionExtent.element(arguments.ActionPath);
    if (actionElement == null)
    {
        TheLog.Error($"Action element at {arguments.ActionPath} could not be found");
        return;
    }

    TheLog.Info($"Action element found: {actionElement}. Now try to execute it");

    using (new StopWatchLogger(new ClassLogger(typeof(Program)), "Action Execution"))
    {
        var actionLogic = GiveMe.Scope.Resolve<ActionLogic>();
        await actionLogic.ExecuteAction(actionElement);
    }

    TheLog.Info($"Action has been executed");
}
finally
{
    GiveMe.Scope.Dispose();

    using (new StopWatchLogger(new ClassLogger(typeof(Program)), "Cleaning up"))
    {
        Directory.Delete(databasePath, true);
    }
}