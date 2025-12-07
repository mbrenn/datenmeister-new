using System.Reflection;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using DatenMeister.Plugins;
using NUnit.Framework;
#if !NET462
#endif

namespace DatenMeister.Tests;

[TestFixture]
public class DatenMeisterTests
{
    public static string GetPathForTemporaryStorage(string fileName)
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            "testing/datenmeister/data");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return Path.Combine(path, fileName);
    }

    /// <summary>
    /// Gets the DatenMeister Scope for the testing
    /// </summary>
    /// <returns></returns>
    public static async Task<IDatenMeisterScope> GetDatenMeisterScope(
        bool dropDatabase = true,
        IntegrationSettings? integrationSettings = null)
    {
        TheLog.ClearProviders();
        TheLog.AddProvider(new ConsoleProvider());
        TheLog.AddProvider(new DebugProvider());
        TheLog.FilterThreshold = LogLevel.Trace;

        integrationSettings ??= GetIntegrationSettings(dropDatabase);

        if (dropDatabase)
        {
            GiveMe.DropDatenMeisterStorage(integrationSettings);
        }

        ExtentConfigurationLoader.BreakOnFailedWorkspaceLoading = false;
        var result = await GiveMe.DatenMeister(integrationSettings);
        var typeIndexLogic = new TypeIndexLogic(result.WorkspaceLogic);
        typeIndexLogic.TypeIndexStore.WaitForAvailabilityOfIndexStore();
        return result;
    }

    public static (IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) GetDmInfrastructure()
    {
        var scopeStorage = new ScopeStorage();
        var workspaceData = WorkspaceLogic.InitDefault();
        scopeStorage.Add(workspaceData);
        var workspaceLogic = new WorkspaceLogic(scopeStorage);

        return (workspaceLogic, scopeStorage);
    }
                
    /// <summary>
    /// Gets the integration settings
    /// </summary>
    /// <param name="dropDatabase">true, if the database shall be dropped</param>
    /// <returns>The created integration settings</returns>
    public static IntegrationSettings GetIntegrationSettings(bool dropDatabase = true)
    {
        var path = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
            throw new InvalidOperationException("Get Directory Name failed"),
            $"testing/datenmeister/data");
        var integrationSettings = new IntegrationSettings
        {
            DatabasePath = path,
            EstablishDataEnvironment = true,
            PerformSlimIntegration = false,
            AllowNoFailOfLoading = false,
            InitializeDefaultExtents = dropDatabase
        };

        return integrationSettings;
    }

    [Test]
    public async Task CheckFailureFreeLoadingOfDatenMeister()
    {
        await using var datenMeister = await GetDatenMeisterScope();
        var pluginManager = datenMeister.ScopeStorage.Get<PluginManager>();
        Assert.That(pluginManager.NoExceptionDuringLoading, Is.True);
    }

    [Test]
    public async Task TestSlimLoading()
    {
        var integrationSettings = GetIntegrationSettings();
        integrationSettings.PerformSlimIntegration = true;

        await using var datenMeister = await GetDatenMeisterScope(integrationSettings: integrationSettings);
        var pluginManager = datenMeister.ScopeStorage.Get<PluginManager>();
        Assert.That(pluginManager.NoExceptionDuringLoading, Is.True);
    }
}