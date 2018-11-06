using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI;
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
            var xmlProvider = new XmiProvider();
            var extent = new MofUriExtent(xmlProvider, "datenmeister:///test/");
            var factory = new MofFactory(extent);
            var mofObject1 = factory.create(null);
            var mofObject2 = factory.create(null);
            var mofObject3 = factory.create(null);
            mofObject1.set("name", "Martin");
            mofObject2.set("name", "Martina");
            mofObject3.set("name", "Martini");
            
            Assert.That(extent.contextURI(), Is.EqualTo("datenmeister:///test/"));

            extent.elements().add(mofObject1);
            extent.elements().add(mofObject2);
            extent.elements().add(mofObject3);

            var xmiStorageConfiguration = new XmiStorageConfiguration
            {
                extentUri = "datenmeister:///test/",
                filePath = "data.xml"
            };

            var xmiStorage = new XmiStorage();
            xmiStorage.StoreProvider(extent.Provider, xmiStorageConfiguration);

            var otherExtent = new MofUriExtent(xmiStorage.LoadProvider(xmiStorageConfiguration), "datenmeister:///tests/");
            Assert.That(otherExtent.elements().size(), Is.EqualTo(3));
            Assert.That(otherExtent.contextURI(), Is.EqualTo("datenmeister:///tests/"));
            Assert.That((otherExtent.elements().ElementAt(0) as IObject)?.get("name"), Is.EqualTo("Martin"));
            Assert.That((otherExtent.elements().ElementAt(1) as IObject)?.get("name"), Is.EqualTo("Martina"));
            Assert.That((otherExtent.elements().ElementAt(2) as IObject)?.get("name"), Is.EqualTo("Martini"));

            File.Delete("data.xml");
        }

        [Test]
        public void TestHrefAttributeLoading()
        {
            const string xmi1 = "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"test\" value=\"23\" /></package>";
            const string xmi2 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"other\" value=\"23\"><sub href=\"datenmeister:///xmi1/#test\" /></element></package>";

            var extent1 = new MofUriExtent(new InMemoryProvider(), "datenmeister:///xmi1/");
            var extent2 = new MofUriExtent(new InMemoryProvider(), "datenmeister:///xmi2/");

            var workspace = new Workspace("data");
            var loader = new SimpleLoader(workspace);
            workspace.AddExtent(extent1);
            workspace.AddExtent(extent2);
            loader.LoadFromText(new MofFactory(extent1), extent1, xmi1);
            loader.LoadFromText(new MofFactory(extent2), extent2, xmi2);

            // Verify correct addressing
            var foundElement = extent1.element("datenmeister:///xmi1/#test");
            Assert.That(foundElement, Is.Not.Null);

            // Now verify the full href loading
            var otherElement = (extent2.elements().FirstOrDefault() as IElement)?.get("sub") as IElement;
            Assert.That(otherElement, Is.Not.Null);
            Assert.That(otherElement.get("value").ToString(), Is.EqualTo("23"));
            Assert.That(otherElement, Is.EqualTo(foundElement));
        }
    }
}