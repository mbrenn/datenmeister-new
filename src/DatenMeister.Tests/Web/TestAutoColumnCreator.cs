using System.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Models.Modules.ViewFinder.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class TestAutoColumnCreator1
    {
        [Test]
        public void TestSimpleAutoColumn()
        {
            var property1 = "zip";
            var property2 = "location";

            var mofObject = new MofObject();
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = new MofObject();
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");

            var extent = new MofUriExtent("datenmeister:///test");
            extent.elements().add(mofObject);
            extent.elements().add(mofObject2);
            var creator = new FormCreator();
            var result = creator.CreateForm(extent);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.fields.Count(), Is.EqualTo(2));
            var firstColumn = result.fields.FirstOrDefault(x => x.name == "zip");
            var secondColumn = result.fields.FirstOrDefault(x => x.name == "location");

            Assert.That(firstColumn, Is.Not.Null);
            Assert.That(secondColumn, Is.Not.Null);

            Assert.That(firstColumn.isEnumeration, Is.False);

            Assert.That(result.fields.Count, Is.EqualTo(2));
            Assert.That(result.fields[0].name, Is.EqualTo("zip"));
            Assert.That(result.fields[1].name, Is.EqualTo("location"));
        }

        [Test]
        public void TestEnumerationAutoColumn()
        {
            var property1 = "zip";
            var property2 = "location";
            var property3 = "other";

            var mofObject = new MofObject();
            mofObject.set(property1, "55130");
            mofObject.set(property2, "Mainz");

            var mofObject2 = new MofObject();
            mofObject2.set(property1, "65474");
            mofObject2.set(property2, "Bischofsheim");

            var enumeration = new MofReflectiveSequence();
            enumeration.add(new MofObject());
            mofObject2.set(property3, enumeration);

            var extent = new MofUriExtent("datenmeister:///test");
            extent.elements().add(mofObject);
            extent.elements().add(mofObject2);

            // Execute the stuff
            var creator = new FormCreator();
            var result = creator.CreateForm(extent);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.fields.Count(), Is.EqualTo(3));
            var firstColumn = result.fields.FirstOrDefault(x => x.name == "zip");
            var secondColumn = result.fields.FirstOrDefault(x => x.name == "location");
            var thirdColumn = result.fields.FirstOrDefault(x => x.name == "other");

            Assert.That(firstColumn, Is.Not.Null);
            Assert.That(secondColumn, Is.Not.Null);

            Assert.That(firstColumn.isEnumeration, Is.False);
            Assert.That(secondColumn.isEnumeration, Is.False);
            Assert.That(thirdColumn.isEnumeration, Is.True);
        }
    }
}