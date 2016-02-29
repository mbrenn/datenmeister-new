using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Filler;
using DatenMeister.XMI.UmlBootstrap;
using NUnit.Framework;

namespace DatenMeister.Tests.Core
{
    [TestFixture]
    public class DataLayerTests
    {
        [Test]
        public void TestDataLayers()
        {
            var data = new DataLayerData();
            IDataLayerLogic logic = new DataLayerLogic(data);
            logic.SetRelationsForDefaultDataLayers();

            var dataExtent = new MofUriExtent("Data");
            var typeExtent = new MofUriExtent("Types");
            var umlExtent = new MofUriExtent("Uml");
            var unAssignedExtent = new MofUriExtent("Unassigned");

            logic.AssignToDataLayer(dataExtent, DataLayers.Data);
            logic.AssignToDataLayer(typeExtent, DataLayers.Types);
            logic.AssignToDataLayer(umlExtent, DataLayers.Uml);

            Assert.That(logic.GetDataLayerOfExtent(dataExtent), Is.EqualTo(DataLayers.Data));
            Assert.That(logic.GetDataLayerOfExtent(typeExtent), Is.EqualTo(DataLayers.Types));
            Assert.That(logic.GetDataLayerOfExtent(umlExtent), Is.EqualTo(DataLayers.Uml));
            Assert.That(logic.GetDataLayerOfExtent(unAssignedExtent), Is.EqualTo(DataLayers.Data));
            Assert.That(logic.GetMetaLayerFor(DataLayers.Data), Is.EqualTo(DataLayers.Types));
        }

        [Test]
        public void TestDataLayersForItem()
        {
            var data = new DataLayerData();
            IDataLayerLogic logic = new DataLayerLogic(data);
            logic.SetRelationsForDefaultDataLayers();

            var dataExtent = new MofUriExtent("Data");
            var umlExtent = new MofUriExtent("Uml");

            logic.AssignToDataLayer(dataExtent, DataLayers.Data);
            logic.AssignToDataLayer(umlExtent, DataLayers.Uml);

            var value = new MofElement(null, null);
            var logicLayer = logic.GetDataLayerOfObject(value);
            Assert.That(logicLayer, Is.SameAs(DataLayers.Data)); // Per Default, only the Data

            umlExtent.elements().add(value);
            logicLayer = logic.GetDataLayerOfObject(value);
            Assert.That(logicLayer, Is.SameAs(DataLayers.Uml));
        }

        [Test]
        public void TestClassTreeUsage()
        {
            var strapper = Bootstrapper.PerformFullBootstrap("Xmi/PrimitiveTypes.xmi", "Xmi/UML.xmi", "Xmi/MOF.xmi");

            var data = new DataLayerData();
            IDataLayerLogic logic = new DataLayerLogic(data);
            logic.SetRelationsForDefaultDataLayers();
            logic.AssignToDataLayer(strapper.PrimitiveInfrastructure, DataLayers.Uml);

            var primitiveTypes = logic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(DataLayers.Uml);
            Assert.That(primitiveTypes, Is.Not.Null );
            Assert.That(primitiveTypes.__Real, Is.Not.Null);
            Assert.That(primitiveTypes.__Real, Is.Not.TypeOf<object>());

            var primitiveTypes2 = logic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(DataLayers.Uml);
            Assert.That(primitiveTypes2, Is.SameAs(primitiveTypes));

            logic.ClearCache(DataLayers.Uml);
            var primitiveTypes3 = logic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(DataLayers.Uml);
            Assert.That(primitiveTypes3, Is.Not.SameAs(primitiveTypes));

            var primitiveTypes4 = logic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(DataLayers.Types);
            Assert.That(primitiveTypes4, Is.Not.Null);
            Assert.That(primitiveTypes4.__Real, Is.TypeOf<object>());
        }
    }
}