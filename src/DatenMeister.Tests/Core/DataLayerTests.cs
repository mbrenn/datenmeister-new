using DatenMeister.Core;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.Filler;
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
            WorkspaceData data;
            var dataLayers = WorkspaceLogic.InitDefault(out data);
            var logic = new WorkspaceLogic(data);

            var dataExtent = new MofUriExtent("Data");
            var typeExtent = new MofUriExtent("Types");
            var umlExtent = new MofUriExtent("Uml");
            var unAssignedExtent = new MofUriExtent("Unassigned");

            logic.AssignToDataLayer(dataExtent, dataLayers.Data);
            logic.AssignToDataLayer(typeExtent, dataLayers.Types);
            logic.AssignToDataLayer(umlExtent, dataLayers.Uml);

            Assert.That(logic.GetDataLayerOfExtent(dataExtent), Is.EqualTo(dataLayers.Data));
            Assert.That(logic.GetDataLayerOfExtent(typeExtent), Is.EqualTo(dataLayers.Types));
            Assert.That(logic.GetDataLayerOfExtent(umlExtent), Is.EqualTo(dataLayers.Uml));
            Assert.That(logic.GetDataLayerOfExtent(unAssignedExtent), Is.EqualTo(dataLayers.Data));
            Assert.That(logic.GetMetaLayerFor(dataLayers.Data), Is.EqualTo(dataLayers.Types));
        }

        [Test]
        public void TestDataLayersForItem()
        {
            WorkspaceData data;
            var dataLayers = WorkspaceLogic.InitDefault(out data);
            var logic = new WorkspaceLogic(data);

            var dataExtent = new MofUriExtent("Data");
            var umlExtent = new MofUriExtent("Uml");

            logic.AssignToDataLayer(dataExtent, dataLayers.Data);
            logic.AssignToDataLayer(umlExtent, dataLayers.Uml);

            var value = new MofElement(null, null);
            var logicLayer = logic.GetDataLayerOfObject(value);
            Assert.That(logicLayer, Is.SameAs(dataLayers.Data)); // Per Default, only the Data

            umlExtent.elements().add(value);
            logicLayer = logic.GetDataLayerOfObject(value);
            Assert.That(logicLayer, Is.SameAs(dataLayers.Uml));
        }

        [Test]
        public void TestClassTreeUsage()
        {
            WorkspaceData data;
            var dataLayers = WorkspaceLogic.InitDefault(out data);
            var dataLayerLogic = new WorkspaceLogic(data);

            var strapper = Bootstrapper.PerformFullBootstrap(
                dataLayerLogic, 
                dataLayers.Uml,
                BootstrapMode.Mof);

            var primitiveTypes = dataLayerLogic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(dataLayers.Uml);
            Assert.That(primitiveTypes, Is.Not.Null );
            Assert.That(primitiveTypes.__Real, Is.Not.Null);
            Assert.That(primitiveTypes.__Real, Is.Not.TypeOf<object>());
            
            var primitiveTypes2 = dataLayerLogic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(dataLayers.Uml);
            Assert.That(primitiveTypes2, Is.SameAs(primitiveTypes));
        }
    }
}