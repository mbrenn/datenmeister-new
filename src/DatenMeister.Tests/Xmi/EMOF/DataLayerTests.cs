using DatenMeister.Core;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.Filler;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml;
using NUnit.Framework;

namespace DatenMeister.Tests.Core
{
    [TestFixture]
    public class DataLayerTests
    {
        [Test]
        public void TestDataLayers()
        {
            var data = WorkspaceLogic.InitDefault();
            var logic = new WorkspaceLogic(data);

            var dataExtent = new MofUriExtent("Data");
            var typeExtent = new MofUriExtent("Types");
            var umlExtent = new MofUriExtent("Uml");
            var unAssignedExtent = new MofUriExtent("Unassigned");

            logic.AssignToDataLayer(dataExtent, data.Data);
            logic.AssignToDataLayer(typeExtent, data.Types);
            logic.AssignToDataLayer(umlExtent, data.Uml);

            Assert.That(logic.GetDataLayerOfExtent(dataExtent), Is.EqualTo(data.Data));
            Assert.That(logic.GetDataLayerOfExtent(typeExtent), Is.EqualTo(data.Types));
            Assert.That(logic.GetDataLayerOfExtent(umlExtent), Is.EqualTo(data.Uml));
            Assert.That(logic.GetDataLayerOfExtent(unAssignedExtent), Is.EqualTo(data.Data));
            Assert.That(data.Data.MetaWorkspace, Is.EqualTo(data.Types));
        }

        [Test]
        public void TestDataLayersForItem()
        {
            var data = WorkspaceLogic.InitDefault();
            var logic = new WorkspaceLogic(data);

            var dataExtent = new MofUriExtent("Data");
            var umlExtent = new MofUriExtent("Uml");

            logic.AssignToDataLayer(dataExtent, data.Data);
            logic.AssignToDataLayer(umlExtent, data.Uml);

            var value = new MofElement(null, null);
            var logicLayer = logic.GetDataLayerOfObject(value);
            Assert.That(logicLayer, Is.SameAs(data.Data)); // Per Default, only the Data

            umlExtent.elements().add(value);
            logicLayer = logic.GetDataLayerOfObject(value);
            Assert.That(logicLayer, Is.SameAs(data.Uml));
        }

        [Test]
        public void TestClassTreeUsage()
        {
            var data = WorkspaceLogic.InitDefault();
            var dataLayerLogic = new WorkspaceLogic(data);

            var strapper = Bootstrapper.PerformFullBootstrap(
                dataLayerLogic,
                data.Uml,
                BootstrapMode.Mof);

            var primitiveTypes = data.Uml.Create<FillThePrimitiveTypes, _PrimitiveTypes>();
            Assert.That(primitiveTypes, Is.Not.Null );
            Assert.That(primitiveTypes.__Real, Is.Not.Null);
            Assert.That(primitiveTypes.__Real, Is.Not.TypeOf<object>());
            
            var primitiveTypes2 = data.Uml.Create<FillThePrimitiveTypes, _PrimitiveTypes>();
            Assert.That(primitiveTypes2, Is.SameAs(primitiveTypes));
        }
    }
}