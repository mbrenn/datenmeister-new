using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules;

[TestFixture]
public class ChangeEventManagerTests
{
    public class CounterClass
    {
        public int ElementCount { get; set; }
        public int ExtentCount { get; set; }
        public int WorkspaceCount { get; set; }

        public override string ToString()
        {
            return
                $"{{ elementCount = {ElementCount}, extentCount = {ExtentCount}, workspaceCount = {WorkspaceCount} }}";
        }
    }

    [Test]
    public async Task TestCallsOfEvents()
    {
        await using var datenMeister = await DatenMeisterTests.GetDatenMeisterScope();
        var manager = datenMeister.ScopeStorage.Get<ChangeEventManager>();
        var workspaceLogic = datenMeister.Resolve<IWorkspaceLogic>();
        var data = workspaceLogic.GetDataWorkspace();

        var extent = new MofUriExtent(new InMemoryProvider(), null);
        workspaceLogic.AddExtent(data, extent);

        var factory = new MofFactory(extent);

        var element = factory.create(null);
        extent.elements().add(element);

        var counter = new CounterClass {ElementCount = 0, ExtentCount = 0, WorkspaceCount = 0};
        manager.RegisterFor(element, _ => counter.ElementCount++);
        manager.RegisterFor(extent, (_, _) => counter.ExtentCount++);
        manager.RegisterFor(data, (_, _, _) => counter.WorkspaceCount++);

        element.set("Test", 1);

        Assert.That(counter.ElementCount, Is.EqualTo(1));
        Assert.That(counter.ExtentCount, Is.EqualTo(1));
        Assert.That(counter.WorkspaceCount, Is.EqualTo(1));

        counter.ElementCount = counter.ExtentCount = counter.WorkspaceCount = 0;

        var element2 = factory.create(null);
        extent.elements().add(element2);

        Assert.That(counter.ElementCount, Is.EqualTo(0));
        Assert.That(counter.ExtentCount, Is.GreaterThanOrEqualTo(1));
        Assert.That(counter.WorkspaceCount, Is.GreaterThanOrEqualTo(2));
    }
}