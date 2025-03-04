﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Locking;
using DatenMeister.Provider.Xmi.Provider.XMI;
using DatenMeister.Provider.Xmi.Provider.XMI.ExtentStorage;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi.Runtime
{
    [TestFixture]
    public class StorageTests
    {
        [Test]
        public async Task TestXmlStorage()
        {
            var xmlProvider = new XmiProvider();
            var extent = new MofUriExtent(xmlProvider, "dm:///test/", null);
            var factory = new MofFactory(extent);
            var mofObject1 = factory.create(null);
            var mofObject2 = factory.create(null);
            var mofObject3 = factory.create(null);
            mofObject1.set("name", "Martin");
            mofObject2.set("name", "Martina");
            mofObject3.set("name", "Martini");
            var lockingState = new LockingState();
            LockingLogic.Create(lockingState);

            Assert.That(extent.contextURI(), Is.EqualTo("dm:///test/"));

            extent.elements().add(mofObject1);
            extent.elements().add(mofObject2);
            extent.elements().add(mofObject3);

            var xmiStorageConfiguration = InMemoryObject.CreateEmpty(
                _DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
            xmiStorageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri,
                "dm:///test");
            xmiStorageConfiguration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
                DatenMeisterTests.GetPathForTemporaryStorage("data.xml"));

            var xmiStorage = new XmiStorageProviderLoader
            {
                ScopeStorage = new ScopeStorage().Add(new ExtentStorageData())
            };

            await xmiStorage.StoreProvider(extent.Provider, xmiStorageConfiguration);

            var otherExtent =
                new MofUriExtent(
                    (await xmiStorage.LoadProvider(xmiStorageConfiguration, ExtentCreationFlags.LoadOnly)).Provider,
                    "dm:///tests/", null);
            Assert.That(otherExtent.elements().size(), Is.EqualTo(3));
            Assert.That(otherExtent.contextURI(), Is.EqualTo("dm:///tests/"));
            Assert.That((otherExtent.elements().ElementAt(0) as IObject)?.get("name"), Is.EqualTo("Martin"));
            Assert.That((otherExtent.elements().ElementAt(1) as IObject)?.get("name"), Is.EqualTo("Martina"));
            Assert.That((otherExtent.elements().ElementAt(2) as IObject)?.get("name"), Is.EqualTo("Martini"));

            File.Delete("data.xml");
        }

        [Test]
        public void TestHrefAttributeLoading()
        {
            const string xmi1 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"test\" value=\"23\" /></package>";
            const string xmi2 =
                "<package xmlns:xmi=\"http://www.omg.org/spec/XMI/20131001\"><element xmi:id=\"other\" value=\"23\"><sub href=\"dm:///xmi1/#test\" /></element></package>";

            var extent1 = new MofUriExtent(new InMemoryProvider(), "dm:///xmi1/", null);
            var extent2 = new MofUriExtent(new InMemoryProvider(), "dm:///xmi2/", null);

            var workspace = new Workspace("data");
            var loader = new SimpleLoader(workspace);
            workspace.AddExtent(extent1);
            workspace.AddExtent(extent2);
            loader.LoadFromText(new MofFactory(extent1), extent1, xmi1);
            loader.LoadFromText(new MofFactory(extent2), extent2, xmi2);

            // Verify correct addressing
            var foundElement = extent1.element("dm:///xmi1/#test");
            Assert.That(foundElement, Is.Not.Null);

            // Now verify the full href loading
            var otherElement = (extent2.elements().FirstOrDefault() as IElement)?.get("sub") as IElement;
            Assert.That(otherElement, Is.Not.Null);
            Assert.That(otherElement?.get("value")?.ToString(), Is.EqualTo("23"));
            Assert.That(otherElement, Is.EqualTo(foundElement));
        }
    }
}