using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi.Runtime
{
    [TestFixture]
    public class StorageTests
    {
        [Test]
        public void TestXmlStorage()
        {
            var xmlProvider = new XmlUriExtent();
            var extent = new MofUriExtent(xmlProvider, "dm:///test/");
            var factory = new MofFactory(extent);
            var mofObject1 = factory.create(null);
            var mofObject2 = factory.create(null);
            var mofObject3 = factory.create(null);
            mofObject1.set("name", "Martin");
            mofObject2.set("name", "Martina");
            mofObject3.set("name", "Martini");
            
            Assert.That(extent.contextURI(), Is.EqualTo("dm:///test/"));

            extent.elements().add(mofObject1);
            extent.elements().add(mofObject2);
            extent.elements().add(mofObject3);

            var xmiStorageConfiguration = new XmiStorageConfiguration
            {
                ExtentUri = "dm:///test/",
                Path = "data.xml"
            };

            var xmiStorage = new XmiStorage(WorkspaceLogic.GetDefaultLogic());
            xmiStorage.StoreExtent(extent.Provider, xmiStorageConfiguration);

            var otherExtent = new MofUriExtent(xmiStorage.LoadExtent(xmiStorageConfiguration), "dm:///tests/");
            Assert.That(otherExtent.elements().size(), Is.EqualTo(3));
            Assert.That(otherExtent.contextURI(), Is.EqualTo("dm:///tests/"));
            Assert.That((otherExtent.elements().ElementAt(0) as IObject)?.get("name"), Is.EqualTo("Martin"));
            Assert.That((otherExtent.elements().ElementAt(1) as IObject)?.get("name"), Is.EqualTo("Martina"));
            Assert.That((otherExtent.elements().ElementAt(2) as IObject)?.get("name"), Is.EqualTo("Martini"));

            File.Delete("data.xml");
        }
    }
}