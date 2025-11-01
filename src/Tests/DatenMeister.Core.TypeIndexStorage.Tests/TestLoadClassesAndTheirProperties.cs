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
    }
}