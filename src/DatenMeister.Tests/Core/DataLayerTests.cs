using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Filler;
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
            var dataLayers = new DataLayers();
            var data = new DataLayerData(dataLayers);
            IDataLayerLogic logic = new DataLayerLogic(data);
            dataLayers.SetRelationsForDefaultDataLayers(logic);

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
            var dataLayers = new DataLayers();
            var data = new DataLayerData(dataLayers);
            IDataLayerLogic logic = new DataLayerLogic(data);
            dataLayers.SetRelationsForDefaultDataLayers(logic);

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
            var dataLayerLogic = new DataLayerLogic(new DataLayerData());
            var dataLayers = new DataLayers();
            dataLayers.SetRelationsForDefaultDataLayers(dataLayerLogic);

            var strapper = Bootstrapper.PerformFullBootstrap(
                new Bootstrapper.FilePaths()
                {
                    PathPrimitive = "Xmi/PrimitiveTypes.xmi",
                    PathUml = "Xmi/UML.xmi",
                    PathMof = "Xmi/MOF.xmi"
                },
                dataLayerLogic, 
                dataLayers.Uml);

            var primitiveTypes = dataLayerLogic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(dataLayers.Uml);
            Assert.That(primitiveTypes, Is.Not.Null );
            /*Assert.That(primitiveTypes.__Real, Is.Not.Null);
            Assert.That(primitiveTypes.__Real, Is.Not.TypeOf<object>());
            */
            var primitiveTypes2 = dataLayerLogic.Create<FillThePrimitiveTypes, _PrimitiveTypes>(dataLayers.Uml);
            Assert.That(primitiveTypes2, Is.SameAs(primitiveTypes));
        }
    }
}