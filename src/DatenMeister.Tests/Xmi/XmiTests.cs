using System.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.XMI;
using NUnit.Framework;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.XMI.UmlBootstrap;
using DatenMeister.EMOF.Queries;
using DatenMeister.Filler;

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
            var strapper = Bootstrapper.PerformFullBootstrap("Xmi/UML.xmi", "Xmi/MOF.xmi");
            Assert.That(strapper, Is.Not.Null);
            Assert.That(strapper.UmlInfrastructure, Is.Not.Null);

            Assert.That(
                AllDescendentsQuery.getDescendents(strapper.UmlInfrastructure).Count(),
                Is.GreaterThan(500));

            // Check, if the filled classes are working
            var mof = new _MOF();
            var uml = new _UML();
            FillTheMOF.DoFill(strapper.MofInfrastructure.elements(), mof);
            FillTheUML.DoFill(strapper.UmlInfrastructure.elements(), uml);
        }
    }
}
