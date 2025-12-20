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
        
        Assert.That(rowFlattenNode!.Generalizations.Count, Is.GreaterThan(0));
        Assert.That(rowFlattenNode.Generalizations.Any(x=>x == _DataViews.TheOne.__ViewNode.Uri));
    }

    [Test]
    public async Task TestGeneralizationsOfProperties()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceTypes);

        var rowFlattenNode = typesWorkspace!.FindClassByUri(_DataViews.TheOne.Row.__RowFlattenNode.Uri);
        Assert.That(rowFlattenNode,Is.Not.Null);
        
        // Test that the direct attributes can be found
        var rowFlattenNodeAttribute = rowFlattenNode!.Attributes.FirstOrDefault(x => x.Name == _DataViews._Row._RowFlattenNode.input);
        var inheritedAttribute = rowFlattenNode.Attributes.FirstOrDefault(x => x.Name == _DataViews._Row._RowFlattenNode.name);
        
        Assert.That(rowFlattenNodeAttribute, Is.Not.Null);
        Assert.That(rowFlattenNodeAttribute!.IsInherited, Is.False);
        
        Assert.That(inheritedAttribute, Is.Not.Null);
        Assert.That(inheritedAttribute!.IsInherited, Is.True);
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

        var attributeCommandLine = classCommandLineApplication!.Attributes.FirstOrDefault(x =>
            x.Name == _CommonTypes._OSIntegration._CommandLineApplication.applicationPath);
        Assert.That(attributeCommandLine, Is.Not.Null);

        Assert.That(attributeCommandLine!.TypeUrl,
            Is.EqualTo(_PrimitiveTypes.TheOne.__String.Uri));
        Assert.That(attributeCommandLine.Id, Is.Not.Null.Or.Empty);

        Assert.That(attributeCommandLine.DefaultValue, Is.Null);
        Assert.That(attributeCommandLine.IsMultiple, Is.Null);
        Assert.That(attributeCommandLine.IsComposite, Is.True);

        var queryStatement = typesWorkspace.ClassModels.FirstOrDefault(x => x.Name == "QueryStatement");
        Assert.That(queryStatement, Is.Not.Null);
        var resultNode = queryStatement!.Attributes.FirstOrDefault(x => x.Name == "resultNode");
        Assert.That(resultNode, Is.Not.Null);

        Assert.That(resultNode!.IsMultiple, Is.False);

        var reportDefinition = typesWorkspace.ClassModels.FirstOrDefault(x => x.Name == "ReportDefinition");
        Assert.That(reportDefinition, Is.Not.Null);
        var reportDefinitionElements = reportDefinition!.Attributes.FirstOrDefault(x => x.Name == "elements");
        Assert.That(reportDefinitionElements, Is.Not.Null);

        Assert.That(reportDefinitionElements!.IsMultiple, Is.True);
    }
    
    [Test]
    public async Task TestWorkspaceAndExtentUriCorrectlySet()
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
        
        // Verify WorkspaceId is set correctly
        Assert.That(classCommandLineApplication!.WorkspaceId, Is.EqualTo(WorkspaceNames.WorkspaceTypes));
        
        // Verify ExtentUri is set correctly. 
        // For CommandLineApplication, it should be in the internal types extent.
        Assert.That(classCommandLineApplication.ExtentUri, Is.Not.Null.And.Not.Empty);
        Assert.That(classCommandLineApplication.ExtentUri, Does.Contain("dm:///_internal/types/internal"));
    }
}