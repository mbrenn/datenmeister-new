using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi.EMOF
{
    [TestFixture]
    public class DataLayerTests
    {
        [Test]
        public void TestDataLayers()
        {
            var data = WorkspaceLogic.InitDefault();
            var logic = WorkspaceLogic.Create(data);

            var dataExtent = new MofUriExtent(new InMemoryProvider(), "Data", null);
            var typeExtent = new MofUriExtent(new InMemoryProvider(), "Types", null);
            var umlExtent = new MofUriExtent(new InMemoryProvider(), "Uml", null);
            var unAssignedExtent = new MofUriExtent(new InMemoryProvider(), "Unassigned", null);

            logic.AddExtent(data.Data, dataExtent);
            logic.AddExtent(data.Types, typeExtent);
            logic.AddExtent(data.Uml, umlExtent);

            Assert.That(logic.GetWorkspaceOfExtent(dataExtent), Is.EqualTo(data.Data));
            Assert.That(logic.GetWorkspaceOfExtent(typeExtent), Is.EqualTo(data.Types));
            Assert.That(logic.GetWorkspaceOfExtent(umlExtent), Is.EqualTo(data.Uml));
            Assert.That(logic.GetWorkspaceOfExtent(unAssignedExtent), Is.EqualTo(data.Data));
            Assert.That(data.Data.MetaWorkspaces.FirstOrDefault(), Is.EqualTo(data.Types));
        }

        [Test]
        public void TestDataLayersForItem()
        {
            var data = WorkspaceLogic.InitDefault();
            var logic = WorkspaceLogic.Create(data);

            var dataExtent = new MofUriExtent(new InMemoryProvider(), "Data", null);
            var umlExtent = new MofUriExtent(new InMemoryProvider(), "Uml", null);

            logic.AddExtent(data.Data, dataExtent);
            logic.AddExtent(data.Uml, umlExtent);

            var factory = new MofFactory(dataExtent);
            var value = factory.create(null);

            var logicLayer = logic.GetWorkspaceOfObject(value);
            Assert.That(logicLayer, Is.SameAs(data.Data)); // Per Default, only the Data

            umlExtent.elements().add(value);
            logicLayer = logic.GetWorkspaceOfObject(value);
            Assert.That(logicLayer, Is.SameAs(data.Uml));
        }
    }
}