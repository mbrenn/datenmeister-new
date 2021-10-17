using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.ChangeEvents;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class ChangeEventManagerTests
    {
        public class CounterClass
        {
            public int elementCount { get; set; }
            public int extentCount { get; set; }
            public int workspaceCount { get; set; }

            public override string ToString()
            {
                return $"{{ elementCount = {elementCount}, extentCount = {extentCount}, workspaceCount = {workspaceCount} }}";
            }
        }

        [Test]
        public void TestCallsOfEvents()
        {
            using var datenMeister  = DatenMeisterTests.GetDatenMeisterScope();
            var manager = datenMeister.ScopeStorage.Get<ChangeEventManager>();
            var workspaceLogic = datenMeister.Resolve<IWorkspaceLogic>();
            var data = workspaceLogic.GetDataWorkspace();

            var extent = new MofUriExtent(new InMemoryProvider());
            workspaceLogic.AddExtent(data, extent);
            
            var factory = new MofFactory(extent);

            var element = factory.create(null);
            extent.elements().add(element);

            var counter = new CounterClass {elementCount = 0, extentCount = 0, workspaceCount = 0};
            manager.RegisterFor(element, x => counter.elementCount++);
            manager.RegisterFor(extent, (x,y) => counter.extentCount++);
            manager.RegisterFor(data, (x,y,z) => counter.workspaceCount++);

            element.set("Test", 1);

            Assert.That(counter.elementCount, Is.EqualTo(1));
            Assert.That(counter.extentCount, Is.EqualTo(1));
            Assert.That(counter.workspaceCount, Is.EqualTo(1));

            counter.elementCount = counter.extentCount = counter.workspaceCount = 0;

            var element2 = factory.create(null);
            extent.elements().add(element2);

            Assert.That(counter.elementCount, Is.EqualTo(0));
            Assert.That(counter.extentCount, Is.GreaterThanOrEqualTo(1));
            Assert.That(counter.workspaceCount, Is.GreaterThanOrEqualTo(2));
        }
    }
}