using System.Reflection;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.DependencyInjection;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;

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
        return await GiveMe.DatenMeister(integrationSettings);
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