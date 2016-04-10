using DatenMeister.EMOF.InMemory;
using DatenMeister.Provider.DotNet;
using DatenMeister.Tests.Xmi;
using NUnit.Framework;

namespace DatenMeister.Tests.DotNet
{
    [TestFixture]
    public class DotNetTests
    {
        [Test]
        public void TestDotNetTypeCreation()
        {
            _MOF mof;
            _UML uml;
            XmiTests.CreateUmlAndMofInstance(out mof, out uml);

            var mofFactory= new MofFactory();
            var dotNetTypeCreator = new DotNetTypeGenerator(mofFactory, uml);
            var dotNetClass = dotNetTypeCreator.CreateTypeFor(typeof (TestClass));

            Assert.That(dotNetClass.get(uml.CommonStructure.NamedElement.name), Is.EqualTo("TestClass"));
        }

        public class TestClass
        {
            public string Title { get; set; }
            public int Number { get; set; }
        }

    }
}