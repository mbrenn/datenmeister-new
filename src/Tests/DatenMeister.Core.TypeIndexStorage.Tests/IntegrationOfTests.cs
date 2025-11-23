using System.Reflection;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.Core.TypeIndexAssembly;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

public class IntegrationOfTests
{
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

        integrationSettings ??= GetIntegrationSettings(dropDatabase);

        if (dropDatabase)
        {
            GiveMe.DropDatenMeisterStorage(integrationSettings);
        }

        ExtentConfigurationLoader.BreakOnFailedWorkspaceLoading = false;
        var dm = await GiveMe.DatenMeister(integrationSettings);
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>();
        Assert.That(typeIndexStore, Is.Not.Null);
        typeIndexStore!.IndexWaitTime = TimeSpan.FromSeconds(0.5);
        
        return dm;
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
}