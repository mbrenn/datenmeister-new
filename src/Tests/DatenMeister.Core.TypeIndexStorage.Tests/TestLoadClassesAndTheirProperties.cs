using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestLoadClassesAndTheirProperties
{
    [Test]
    public async Task TestFindClass()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
        Assert.That(typesWorkspace, Is.Not.Null);

        var classCommandLineApplication =
            typesWorkspace!.ClassModels
                .FirstOrDefault(x => x.Name == "CommandLineApplication");
        Assert.That(classCommandLineApplication, Is.Not.Null);
        Assert.That(
            classCommandLineApplication!.Name,
            Is.EqualTo("CommandLineApplication"));
        Assert.That(
            classCommandLineApplication.FullName, 
            Is.EqualTo("DatenMeister::CommonTypes::OSIntegration::CommandLineApplication"));
        Assert.That(
            classCommandLineApplication.Id,
            Is.EqualTo("CommonTypes.OSIntegration.CommandLineApplication"));
        Assert.That(
            classCommandLineApplication.Uri,
            Is.EqualTo(_CommonTypes.TheOne.OSIntegration.__CommandLineApplication.Uri));
    }

    [Test]
    public async Task TestGeneralizationsInClassModel()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);

        var rowFlattenNode = typesWorkspace!.FindClassByUri(_DataViews.TheOne.Row.__RowFlattenNode.Uri);
        Assert.That(rowFlattenNode,Is.Not.Null);
    }
    
    [Test]
    public async Task TestFindProperty()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);
        Assert.That(typesWorkspace, Is.Not.Null);
        
        var classCommandLineApplication =
            typesWorkspace!.ClassModels
                .FirstOrDefault(x => x.Name == "CommandLineApplication");
        Assert.That(classCommandLineApplication, Is.Not.Null);

        var attributeCommandLine = classCommandLineApplication!.Attributes.FirstOrDefault(
            x  => x.Name == _CommonTypes._OSIntegration._CommandLineApplication.applicationPath);
        Assert.That(attributeCommandLine, Is.Not.Null);
        
        Assert.That(attributeCommandLine!.TypeUrl, 
            Is.EqualTo(_PrimitiveTypes.TheOne.__String.Uri));
        Assert.That(attributeCommandLine.Id, Is.Not.Null.Or.Empty);

        Assert.That(attributeCommandLine.DefaultValue, Is.Null);
        Assert.That(attributeCommandLine.IsMultiple, Is.False);
        Assert.That(attributeCommandLine.IsComposite, Is.True);
    }
}