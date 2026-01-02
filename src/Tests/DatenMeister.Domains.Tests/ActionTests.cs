using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;
using NUnit.Framework;

namespace DatenMeister.Domains.Tests;

[TestFixture]
public class ActionTests
{
    [Test]
    public async Task CheckThatDomainsAreLoaded()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var pluginManager = dm.ScopeStorage.Get<PluginManager>();
        Assert.That(
            pluginManager.InstantiatedPlugins.Any(x => x.GetType() == typeof(DomainPlugin)),
            Is.True,
            "DomainPlugin was not loaded.");

        // Check, that the two loaded extents from DomainPlugin 
        var workspaceLogic = dm.WorkspaceLogic;
        Assert.That(
            workspaceLogic.GetTypesWorkspace().extent.OfType<MofUriExtent>()
                .Any(extent => extent.Uri.Contains(DomainPlugin.DmInternTypesDomainsDatenmeister)),
            Is.True,
            "Types Extent was not loaded");
        
        Assert.That(
            workspaceLogic.GetManagementWorkspace().extent.OfType<MofUriExtent>()
                .Any(extent => extent.Uri.Contains(DomainPlugin.DmInternManagementDomainsDatenmeister)),
            Is.True,
            "Management Extent was not loaded");
    }
}