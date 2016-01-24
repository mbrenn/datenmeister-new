using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.XMI;
using NUnit.Framework;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.XMI.UmlBootstrap;
using DatenMeister.EMOF.Queries;
using DatenMeister.Filler;
using DatenMeister.XMI.Standards;

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
            var factory = new DatenMeister.EMOF.InMemory.MofFactory();
            var extent = new MofUriExtent("datenmeister:///target");
            Assert.That(extent.elements().Count(), Is.EqualTo(0));
            var loader = new SimpleLoader(factory);
            loader.Load(extent, "Xmi/UML.xmi");

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

        /// <summary>
        /// Creates a filled MOF and UML instance which can be used for further testing
        /// </summary>
        /// <param name="mof">Mof instance to be returned</param>
        /// <param name="uml">Uml instance to be returned</param>
        public static void CreateUmlAndMofInstance(out _MOF mof, out _UML uml)
        {
            var strapper = Bootstrapper.PerformFullBootstrap("Xmi/PrimitiveTypes.xmi", "Xmi/UML.xmi", "Xmi/MOF.xmi");
            Assert.That(strapper, Is.Not.Null);
            Assert.That(strapper.UmlInfrastructure, Is.Not.Null);

            Assert.That(
                AllDescendentsQuery.getDescendents(strapper.UmlInfrastructure).Count(),
                Is.GreaterThan(500));

            // Check, if the filled classes are working
            mof = new _MOF();
            uml = new _UML();
            FillTheMOF.DoFill(strapper.MofInfrastructure.elements(), mof);
            FillTheUML.DoFill(strapper.UmlInfrastructure.elements(), uml);
        }

        [Test]
        public void TestIdValidity()
        {
            var factory = new MofFactory();

            var document = XDocument.Load("Xmi/MOF.xmi");
            Assert.That(XmiId.IsValid(document), Is.True);

            // Adds an artificial node, which duplicates an id. 
            document.Root.Add(
                new XElement ("other", new XAttribute(Namespaces.Xmi + "id", "_MOF-Identifiers-Extent")));

            Assert.That(XmiId.IsValid(document), Is.False);
        }
    }
}
