using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Helper;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Uml;
using DatenMeister.XMI;
using DatenMeister.XMI.Standards;
using NUnit.Framework;

namespace DatenMeister.Tests.Xmi
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für MofObjectTests
    /// </summary>
    [TestFixture]
    public class XmiTests
    {
        [Test]
        public void LoadUmlInfrastructure()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var factory = new MofFactory();
            var extent = new MofUriExtent("datenmeister:///target");
            Assert.That(extent.elements().Count(), Is.EqualTo(0));
            var loader = new SimpleLoader(factory);
            loader.LoadFromFile(extent, "Xmi/UML.xmi");

            var firstElement = (extent.elements().ElementAt(0) as IObject);
            Assert.That(firstElement, Is.Not.Null);
            Assert.That(firstElement.get("name").ToString(), Is.EqualTo("UML"));
        }

        [Test]
        public void TestBootstrap()
        {
            _MOF mof;
            _UML uml;
            CreateUmlAndMofInstance(out mof, out uml);
        }

        [Test]
        public void TestIdValidity()
        {
            var document = XDocument.Load("Xmi/MOF.xmi");
            Assert.That(XmiId.IsValid(document), Is.True);

            // Adds an artificial node, which duplicates an id. 
            document.Root.Add(
                new XElement ("other", new XAttribute(Namespaces.Xmi + "id", "_MOF-Identifiers-Extent")));

            Assert.That(XmiId.IsValid(document), Is.False);
        }

        [Test]
        public void TestGetUriAndRetrieveElement()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            WorkspaceData data;
            var dataLayers = WorkspaceLogic.InitDefault(out data);
            var dataLayerLogic = new WorkspaceLogic(data);


            var strapper = Bootstrapper.PerformFullBootstrap(dataLayerLogic,
                dataLayers.Uml, 
                BootstrapMode.Mof,
                new Bootstrapper.FilePaths
                {
                    PathPrimitive = "Xmi/PrimitiveTypes.xmi",
                    PathUml = "Xmi/UML.xmi",
                    PathMof = "Xmi/MOF.xmi"
                });
            var umlExtent = strapper.UmlInfrastructure;
            var element = umlExtent.elements().ElementAt(0) as IElement;

            var elementUri = umlExtent.uri(element);
            var foundElement = umlExtent.element(elementUri);
            Assert.That(foundElement, Is.Not.Null);
            Assert.That(foundElement, Is.EqualTo(element));

            // Retrieve another element
            element = AllDescendentsQuery.GetDescendents(umlExtent).ElementAt(300) as IElement;
            elementUri = umlExtent.uri(element);
            foundElement = umlExtent.element(elementUri);
            Assert.That(foundElement, Is.Not.Null);
            Assert.That(foundElement, Is.EqualTo(element));
        }

        [Test]
        public void TestThatPropertyIsIElement()
        {
            var uml = GetFilledUml();

            Assert.That(uml.CommonStructure.__Comment, Is.InstanceOf<IElement>());
            Assert.That(_UML._CommonStructure._Comment.body, Is.InstanceOf<string>());
            Assert.That(uml.CommonStructure.Comment._body, Is.InstanceOf<IElement>());

            Assert.That(
                uml.CommonStructure.Comment._body
                    .isSet(_UML._CommonStructure._NamedElement.name),
                Is.True);

            Assert.That(
                uml.CommonStructure.Comment._body
                    .get(_UML._CommonStructure._NamedElement.name),
                Is.Not.Null);
        }

        [Test]
        public void TestThatGeneralizationsAreOk()
        {
            var uml = GetFilledUml();
            var package = uml.Packages.__Package;

            // Old behavior
            IEnumerable<object> generalizedElements;
            string generalProperty;
            if (package.isSet("generalization"))
            {
                generalizedElements = package.get("generalization") as IEnumerable<object>;
                generalProperty = "general";
            }
            else
            {
                generalizedElements = package.get(
                    _UML._Classification._Classifier.generalization) as IEnumerable<object>;
                generalProperty = _UML._Classification._Generalization.general;
                throw new InvalidOperationException("Not supported at the moment");
            }

            Assert.That(generalizedElements, Is.Not.Null);
            Assert.That(generalizedElements.All(x => x is IElement));
            Assert.That(generalizedElements.Count(), Is.EqualTo(3));

            var firstElement = generalizedElements.ElementAtOrDefault(0) as IElement;
            Assert.That(firstElement,Is.Not.Null);
            var generalContent = firstElement.get(generalProperty);
            Assert.That(generalContent,Is.InstanceOf<IElement>());
            Assert.That(generalContent,Is.EqualTo(uml.CommonStructure.__PackageableElement));
        }

        /// <summary>
        /// Creates a filled MOF and UML instance which can be used for further testing
        /// </summary>
        /// <param name="mof">Mof instance to be returned</param>
        /// <param name="uml">Uml instance to be returned</param>
        public static void CreateUmlAndMofInstance(out _MOF mof, out _UML uml)
        {
            WorkspaceData data;
            var dataLayers = WorkspaceLogic.InitDefault(out data);
            var dataLayerLogic = new WorkspaceLogic(data);
            var strapper = Bootstrapper.PerformFullBootstrap(
                dataLayerLogic,
                dataLayers.Mof,
                BootstrapMode.Mof);
            Assert.That(strapper, Is.Not.Null);
            Assert.That(strapper.UmlInfrastructure, Is.Not.Null);

            Assert.That(
                AllDescendentsQuery.GetDescendents(strapper.UmlInfrastructure).Count(),
                Is.GreaterThan(500));

            // Check, if the filled classes are working
            mof = dataLayerLogic.Get<_MOF>(dataLayers.Mof);
            uml = dataLayerLogic.Get<_UML>(dataLayers.Mof);
            Assert.That(mof, Is.Not.Null);
            Assert.That(uml, Is.Not.Null);
        }

        private static _UML GetFilledUml()
        {
            WorkspaceData data;
            var dataLayers = WorkspaceLogic.InitDefault(out data);
            var dataLayerLogic = new WorkspaceLogic(data);
            Bootstrapper.PerformFullBootstrap(
                dataLayerLogic,
                dataLayers.Mof, 
                BootstrapMode.Mof,
                new Bootstrapper.FilePaths
                {
                    PathPrimitive = "Xmi/PrimitiveTypes.xmi",
                    PathUml = "Xmi/UML.xmi",
                    PathMof = "Xmi/MOF.xmi"
                });

            return dataLayerLogic.Get<_UML>(dataLayers.Mof);
        }
    }
}
