using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestFindModels
{
    [Test]
    public async Task TestFindInLogic()
    {
        
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var indexStore = typeIndexStore.GetCurrentIndexStore();
        var classModel = indexStore.FindClassModel(WorkspaceNames.WorkspaceTypes,
            _CommonTypes.TheOne.OSIntegration.__CommandLineApplication.Uri);
        
        Assert.That(classModel, Is.Not.Null);

    }

    [Test]
    public async Task TestFindInExtent()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
        
    }
    
    [Test]
    public async Task TestFindByInMemoryObjectProvider()
    {
        
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
    }
    
    [Test]
    public async Task TestFindByObject()
    {
        
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
    }
    
    
    
}