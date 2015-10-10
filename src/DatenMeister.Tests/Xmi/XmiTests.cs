using System.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.XMI;
using NUnit.Framework;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.XMI.UmlBootstrap;
using DatenMeister.EMOF.Queries;

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
            var loader = new Loader(extent, factory);
            loader.Load("Xmi/Infrastructure.xml");

            var firstElement = (extent.elements().ElementAt(0) as IObject);
            Assert.That(firstElement, Is.Not.Null);
            Assert.That(firstElement.get("name").ToString(), Is.EqualTo("InfrastructureLibrary"));
        }

        [Test]
        public void TestBootstrap()
        {
            var strapper = Bootstrapper.PerformFullBootstrap("Xmi/Infrastructure.xml");
            Assert.That(strapper, Is.Not.Null);
            Assert.That(strapper.UmlInfrastructure, Is.Not.Null);

            Assert.That(
                AllDescendentsQuery.getDescendents(strapper.UmlInfrastructure).Count(),
                Is.GreaterThan(500));

            Assert.That(strapper.theClassObject, Is.Not.Null);
            Assert.That(strapper.theClassObject.get("name").ToString(), Is.EqualTo("Class"));
        }
    }
}
