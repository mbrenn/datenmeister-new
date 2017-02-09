using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
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

            var typeExtent = DotNetExtentTests.Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            extent.Resolver.AddMetaExtent(typeExtent);
            var strapper = XmiTests.CreateUmlAndMofInstance(out mof, out uml);
            extent.Resolver.AddMetaExtent(strapper.UmlInfrastructure);
            extent.Resolver.AddMetaExtent(strapper.MofInfrastructure);
            extent.Resolver.AddMetaExtent(strapper.PrimitiveTypesInfrastructure);

            var mofFactory = new MofFactory(typeExtent);
            var dotNetTypeCreator = new DotNetTypeGenerator(mofFactory, uml);
            var dotNetClass = dotNetTypeCreator.CreateTypeFor(typeof(TestClass));

            Assert.That(dotNetClass.get(_UML._CommonStructure._NamedElement.name), Is.EqualTo("TestClass"));
        }

        public class TestClass
        {
            public string Title { get; set; }
            public int Number { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }
            public string Prename { get; set; }
        }

        public class TestClassWithList
        {
            public string Title { get; set; }
            public int Number { get; set; }
            public List<string> Authors {get;set; } = new List<string>();
            public List<Person> Persons { get; set; } = new List<Person>();
        }

        public class PersonWithParent
        {
            public string Name { get; set; }
            public string Prename { get; set; }
            public PersonWithParent Parent { get; set; }
        }
    }
}