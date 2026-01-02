using DatenMeister.Plugins;
using NUnit.Framework;

namespace DatenMeister.Domains.Tests;

[TestFixture]
public class ActionTests
{
    [Test]
    public async Task CheckThatDomainsAreLoaded()
    {
        var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var pluginManager = dm.ScopeStorage.Get<PluginManager>();
        Assert.That(
            pluginManager.InstantiatedPlugins.Any(x => x.GetType() == typeof(DomainPlugin)),
            Is.True, 
            "DomainPlugin was not loaded.");
    }
}