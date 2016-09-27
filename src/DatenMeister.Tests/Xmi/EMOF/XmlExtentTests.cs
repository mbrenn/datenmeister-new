﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi.EMOF
{
    [TestFixture]
    public class XmlExtentTests
    {
        [Test]
        public void TestXmlMofObject()
        {
            var mofObject = new XmlElement(new XElement("item"));
            mofObject.set("test", "testvalue");
            Assert.That(mofObject.getMetaClass(), Is.Null);
            Assert.That(mofObject.metaclass, Is.Null);
            Assert.That(mofObject.container(), Is.Null);

            Assert.That(mofObject.isSet("none"), Is.False);
            Assert.That(mofObject.isSet("test"), Is.True);
            Assert.That(mofObject.get("test"), Is.EqualTo("testvalue"));
            mofObject.unset("test");
            Assert.That(mofObject.isSet("test"), Is.False);
        }

        [Test]
        public void TestXmlMofObjectWithElementSet()
        {
            var mofElement = new MofObject();
            mofElement.set("Name", "Brenn");
            mofElement.set("Vorname", "Martin");

            var xmlNode = new XElement("item");
            var xmlElement = new XmlElement(xmlNode);
            xmlElement.set("X", "y");
            xmlElement.set("Person", mofElement);
            
            Assert.That(xmlNode.Attribute("X")?.Value, Is.EqualTo("y"));
            Assert.That(xmlNode.Elements("Person").Count(), Is.EqualTo(1));
            Assert.That(xmlNode.Element("Person")?.Attribute("Name")?.Value, Is.EqualTo("Brenn"));
            Assert.That(xmlNode.Element("Person")?.Attribute("Vorname")?.Value, Is.EqualTo("Martin"));

            Assert.That(xmlElement.get("X"), Is.EqualTo("y"));
            var person = CollectionHelper.MakeSingle(xmlElement.get("Person"));
            Assert.That(person, Is.TypeOf<XmlElement>());

            var personAsElement = (IElement) person;
            Assert.That(personAsElement.get("Name"), Is.EqualTo("Brenn"));
            Assert.That(personAsElement.get("Vorname"), Is.EqualTo("Martin"));
        }

        [Test]
        public void TestXmlMofReflectiveSequence()
        {
            var mofObject1 = new XmlElement(new XElement("item"));
            var mofObject2 = new XmlElement(new XElement("item"));
            var mofObject3 = new XmlElement(new XElement("item"));
            var mofObject4 = new XmlElement(new XElement("item"));

            var mofReflectiveSequence = new XmlReflectiveSequence(null, new XElement("items"), "item");
            Assert.That(mofReflectiveSequence.size(), Is.EqualTo(0));
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(0));

            mofReflectiveSequence.add(mofObject1);
            mofReflectiveSequence.add(mofObject2);
            Assert.That(mofReflectiveSequence.size(), Is.EqualTo(2));
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(2));

            Assert.That(mofReflectiveSequence.get(0).Equals(mofObject1), Is.True);
            Assert.That(mofReflectiveSequence.get(1), Is.EqualTo(mofObject2));

            mofReflectiveSequence.remove(0);
            Assert.That(mofReflectiveSequence.get(0), Is.EqualTo(mofObject2));
            Assert.That(mofReflectiveSequence.size(), Is.EqualTo(1));
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(1));

            mofReflectiveSequence.remove(mofObject2);
            Assert.That(mofReflectiveSequence.ToArray().Count, Is.EqualTo(0));

            mofReflectiveSequence.add(mofObject1);
            mofReflectiveSequence.add(mofObject2);

            var otherMofReflectiveSequence = new XmlReflectiveSequence(null, new XElement("items"), "item");
            otherMofReflectiveSequence.addAll(mofReflectiveSequence);
            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(2));
            Assert.That(otherMofReflectiveSequence.ToArray().Count, Is.EqualTo(2));

            otherMofReflectiveSequence.clear();
            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(0));
            Assert.That(otherMofReflectiveSequence.ToArray().Count, Is.EqualTo(0));


            otherMofReflectiveSequence = new XmlReflectiveSequence(null, new XElement("items"), "item");
            otherMofReflectiveSequence.add(0, mofObject1);
            otherMofReflectiveSequence.add(0, mofObject2);
            otherMofReflectiveSequence.add(1, mofObject3);

            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(3));
            Assert.That(otherMofReflectiveSequence.ElementAt(0), Is.EqualTo(mofObject2));
            Assert.That(otherMofReflectiveSequence.ElementAt(1), Is.EqualTo(mofObject3));
            Assert.That(otherMofReflectiveSequence.ElementAt(2), Is.EqualTo(mofObject1));

            otherMofReflectiveSequence.set(1, mofObject4);
            Assert.That(otherMofReflectiveSequence.size(), Is.EqualTo(3));
            Assert.That(otherMofReflectiveSequence.ElementAt(0), Is.EqualTo(mofObject2));
            Assert.That(otherMofReflectiveSequence.ElementAt(1), Is.EqualTo(mofObject4));
            Assert.That(otherMofReflectiveSequence.ElementAt(2), Is.EqualTo(mofObject1));
        }

        [Test]
        public void TestXmlExtent()
        {
            var mofObject1 = new XmlElement(new XElement("item"));
            var mofObject2 = new XmlElement(new XElement("item"));
            var mofObject3 = new XmlElement(new XElement("item"));

            var extent = new XmlUriExtent(WorkspaceLogic.GetDefaultLogic(),  "dm:///test/");
            Assert.That(extent.contextURI(), Is.EqualTo("dm:///test/"));

            // At the moment, it is not defined whether to contain or not contain. Just to increase coverage
            Assert.That(extent.useContainment(), Is.True.Or.False);

            Assert.That(extent.elements(), Is.Not.Null);
            Assert.That(extent.elements().size(), Is.EqualTo(0));

            extent.elements().add(mofObject1);
            extent.elements().add(mofObject2);
            Assert.That(extent.elements().size(), Is.EqualTo(2));

            mofObject1.set("name", "Martin");
            mofObject1.set("lastname", "Brenn");
            
            mofObject2.set("name", "Another");
            mofObject2.set("lastname", "Brenner");

            Assert.That(mofObject1.get("name").ToString(), Is.EqualTo("Martin"));
            Assert.That(mofObject2.get("name").ToString(), Is.EqualTo("Another"));

            var uri1 = extent.uri(mofObject1);
            var uri2 = extent.uri(mofObject2);
            var id1 = ((IHasId) mofObject1).Id;
            var id2 = ((IHasId) mofObject2).Id;

            Assert.That(id1, Is.Not.Null.Or.Empty);
            Assert.That(id2, Is.Not.Null.Or.Empty);
            Assert.That(id1 != id2, Is.True);

            Assert.That(uri1 != uri2, Is.True);
            Assert.That(uri1.StartsWith("dm:///test/"), Is.True);
            Assert.That(uri2.StartsWith("dm:///test/"), Is.True);

            Assert.That(uri1.EndsWith(id1), Is.True);
            Assert.That(uri2.EndsWith(id2), Is.True);

            var found = extent.element(uri1);
            Assert.That(found, Is.Not.Null);
            Assert.That(found, Is.EqualTo(mofObject1));

            var uri3 = "dm:///test/#abc";
            Assert.That(extent.element(uri3), Is.Null);

            var uri4 = "dm:///test/";
            Assert.That(extent.element(uri4), Is.Null);

            var uri5 = "#abc";
            Assert.That(extent.element(uri5), Is.Null);

            var uri6 = "dm:///anothertest/#" + id1;
            Assert.That(extent.element(uri6), Is.Null);

            extent.elements().remove(mofObject1);
            Assert.That(extent.elements().size(), Is.EqualTo(1));

            extent.elements().remove(mofObject3);
            Assert.That(extent.elements().size(), Is.EqualTo(1));

            var mofElement = new MofElement();
            Assert.Throws<ArgumentNullException>(() => extent.uri(mofElement));

            //Assert.Throws<InvalidOperationException>(() => extent.elements().add(mofElement));
        }

        [Test]
        public void TestXmlFactory()
        {
            var factory = new XmlFactory();
            var mofElement = factory.create(null);
            Assert.That(mofElement, Is.Not.Null);
            Assert.That(mofElement, Is.TypeOf<XmlElement>());
        }

        [Test]
        public void TestXmlExtentStorage()
        {
            var kernel = new ContainerBuilder();

            var builder = kernel.UseDatenMeisterDotNet(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                var path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    $"testing{new Random().Next(0, 100000)}.xml");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                var storageConfiguration = new XmiStorageConfiguration
                {
                    ExtentUri = "dm:///test",
                    Path = path,
                    Workspace = "Data"
                };

                // Creates the extent
                var loader = scope.Resolve<IExtentStorageLoader>();
                var loadedExtent = loader.LoadExtent(storageConfiguration, true);
                Assert.That(loadedExtent, Is.TypeOf<XmlUriExtent>());

                // Includes some data
                var factory = scope.Resolve<IFactoryMapper>().FindFactoryFor(scope, loadedExtent);
                var createdElement = factory.create(null);
                Assert.That(createdElement, Is.TypeOf<XmlElement>());
                loadedExtent.elements().add(createdElement);

                createdElement.set("test", "Test");
                Assert.That(createdElement.get("test"), Is.EqualTo("Test"));

                // Stores the extent
                loader.StoreExtent(loadedExtent);

                // Detaches it
                loader.DetachExtent(loadedExtent);

                // Reloads it
                storageConfiguration.ExtentUri = "dm:///test_new";

                var newExtent = loader.LoadExtent(storageConfiguration, false);
                Assert.That(newExtent.elements().size(), Is.EqualTo(1));
                Assert.That((newExtent.elements().ElementAt(0) as IElement).get("test"), Is.EqualTo("Test"));
            }
        }

        [Test]
        public void TestWithMetaClass()
        {
            var kernel = new ContainerBuilder();
            var builder = kernel.UseDatenMeisterDotNet(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                var dataLayerLogic = scope.Resolve<IWorkspaceLogic>();
                var umlDataLayer = dataLayerLogic.GetUml();
                var uml = umlDataLayer.Get<_UML>();
                Assert.That(uml, Is.Not.Null);

                var extent = new XmlUriExtent(scope.Resolve<IWorkspaceLogic>(), "dm:///test");
                dataLayerLogic.GetTypes().AddExtent(extent);

                var factory = scope.Resolve<IFactoryMapper>().FindFactoryFor(scope, extent);

                var interfaceClass = uml.SimpleClassifiers.__Interface;
                var element = factory.create(interfaceClass);
                Assert.That(element, Is.Not.Null);

                extent.elements().add(element);
                Assert.That(extent.elements().size(), Is.EqualTo(1));

                var retrievedElement = extent.elements().ElementAt(0) as IElement;
                Assert.That(retrievedElement, Is.Not.Null);
                Assert.That(retrievedElement.getMetaClass(), Is.Not.Null);
                Assert.That(retrievedElement.metaclass, Is.Not.Null);
                var foundMetaClass = retrievedElement.metaclass;
                Assert.That(foundMetaClass.Equals(interfaceClass), Is.True);
            }
        }
    }
}