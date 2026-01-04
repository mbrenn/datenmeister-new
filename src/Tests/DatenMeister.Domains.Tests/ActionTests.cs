using DatenMeister.Actions;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Domains.Model;
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

    [Test]
    public async Task CheckCreationOfDomainWithData()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();

        var domainCreate = new Root.DomainCreateFoundationAction_Wrapper(InMemoryObject.TemporaryFactory)
        {
            name = "Test",
            filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DatenMeister", "Domains", "Test"),
            createDataExtent = true,
            extentUriPrefix = "prefix",
            extentUriPostfix = "postfix"
        };
        
        var managementFilePath = Path.Combine(domainCreate.filePath, "Test_Management.xmi");
        var typesFilePath = Path.Combine(domainCreate.filePath, "Test_Types.xmi");
        var dataFilePath = Path.Combine(domainCreate.filePath, "Test_Data.xmi");
        
        // Deletes the files of Management, Types and Data, if they are existing
        if(File.Exists(managementFilePath)) File.Delete(managementFilePath);
        if(File.Exists(typesFilePath))File.Delete(typesFilePath);
        if(File.Exists(dataFilePath))File.Delete(dataFilePath);

        // Now execute the action and verifies that
        // 1. The Data, Management and Types Extents are loaded
        // 2. The files are existing
        var actionHandler = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);
        await actionHandler.ExecuteAction(domainCreate.GetWrappedElement());
        
        Assert.That(
            dm.WorkspaceLogic.GetManagementWorkspace()
                .extent.OfType<MofUriExtent>().Any(x=> "dm:///prefix.management.Test.postfix" == x.Uri), 
            Is.True);
        Assert.That(
            dm.WorkspaceLogic.GetTypesWorkspace()
                .extent.OfType<MofUriExtent>().Any(x=> "dm:///prefix.types.Test.postfix" == x.Uri), 
            Is.True);
        Assert.That(
            dm.WorkspaceLogic.GetDataWorkspace()
                .extent.OfType<MofUriExtent>().Any(x=> "dm:///prefix.data.Test.postfix" == x.Uri), 
            Is.True);
        
        Assert.That(File.Exists(managementFilePath), Is.True);
        Assert.That(File.Exists(typesFilePath), Is.True);
        Assert.That(File.Exists(dataFilePath), Is.True);
        
        // Delete the created files
        if(File.Exists(managementFilePath)) File.Delete(managementFilePath);
        if(File.Exists(typesFilePath))File.Delete(typesFilePath);
        if(File.Exists(dataFilePath))File.Delete(dataFilePath);
    }
    
    [Test]
    public async Task CheckCreationOfDomainWithoutData()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();

        var domainCreate = new Root.DomainCreateFoundationAction_Wrapper(InMemoryObject.TemporaryFactory)
        {
            name = "Test",
            filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DatenMeister", "Domains", "Test"),
            createDataExtent = false,
            extentUriPrefix = "prefix",
            extentUriPostfix = "postfix"
        };
        
        var managementFilePath = Path.Combine(domainCreate.filePath, "Test_Management.xmi");
        var typesFilePath = Path.Combine(domainCreate.filePath, "Test_Types.xmi");
        var dataFilePath = Path.Combine(domainCreate.filePath, "Test_Data.xmi");
        
        // Deletes the files of Management, Types and Data, if they are existing
        if(File.Exists(managementFilePath)) File.Delete(managementFilePath);
        if(File.Exists(typesFilePath))File.Delete(typesFilePath);
        if(File.Exists(dataFilePath))File.Delete(dataFilePath);

        // Now execute the action and verifies that
        // 1. The Data, Management and Types Extents are loaded
        // 2. The files are existing
        var actionHandler = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);
        await actionHandler.ExecuteAction(domainCreate.GetWrappedElement());
        
        Assert.That(
            dm.WorkspaceLogic.GetManagementWorkspace()
                .extent.OfType<MofUriExtent>().Any(x=> "dm:///prefix.management.Test.postfix" == x.Uri), 
            Is.True);
        Assert.That(
            dm.WorkspaceLogic.GetTypesWorkspace()
                .extent.OfType<MofUriExtent>().Any(x=> "dm:///prefix.types.Test.postfix" == x.Uri), 
            Is.True);
        Assert.That(
            dm.WorkspaceLogic.GetDataWorkspace()
                .extent.OfType<MofUriExtent>().Any(x=> "dm:///prefix.data.Test.postfix" == x.Uri), 
            Is.False);
        
        Assert.That(File.Exists(managementFilePath), Is.True);
        Assert.That(File.Exists(typesFilePath), Is.True);
        Assert.That(File.Exists(dataFilePath), Is.False);
        
        // Delete the created files
        if(File.Exists(managementFilePath)) File.Delete(managementFilePath);
        if(File.Exists(typesFilePath))File.Delete(typesFilePath);
        if(File.Exists(dataFilePath))File.Delete(dataFilePath);
    }
}