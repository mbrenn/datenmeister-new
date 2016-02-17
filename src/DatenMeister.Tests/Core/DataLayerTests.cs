using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
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
    }
}