using DatenMeister.EMOF.InMemory;
using DatenMeister.Provider.DotNet;
using NUnit.Framework;

namespace DatenMeister.Tests.DotNet
{
    [TestFixture]
    public class DotNetExtentTests
    {
        [Test]
        public void TestValue()
        {
            var uml = new _UML();
            var mofFactory = new MofFactory();
            var dotNetTypeLookup = new DotNetTypeLookup();
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.TestClass));
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.Person));
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.TestClassWithList));


            var value = new DotNetTests.TestClass();
            var dotNetElement = dotNetTypeLookup.CreateDotNetElement(value);

            Assert.That(dotNetElement.get("Title"), Is.Null);
            Assert.That(dotNetElement.get("Number"), Is.Not.Null);
            Assert.That(dotNetElement.get("Number"), Is.EqualTo(0));

            dotNetElement.set("Title", "Your Movie");
            dotNetElement.set("Number", 3);

            Assert.That(value.Title, Is.EqualTo("Your Movie"));
            Assert.That(value.Number, Is.EqualTo(3));
        }
    }
}