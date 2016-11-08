using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.DotNet
{
    [TestFixture]
    public class DotNetExtentTests
    {
        [Test]
        public void TestValue()
        {
            var dotNetTypeLookup = Initialize();
            
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

        [Test]
        public void TestListWithStrings()
        {
            var dotNetTypeLookup = Initialize();
            var value = new DotNetTests.TestClassWithList();
            var dotNetElement = dotNetTypeLookup.CreateDotNetElement(value);

            var result = dotNetElement.get("Authors");
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IReflectiveSequence>());

            var list = result as IReflectiveSequence;

            Assert.That(list, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(list.size(), Is.EqualTo(0));

            list.add("Mr. M");
            list.add("Mr. B");
            Assert.That(list.size(), Is.EqualTo(2));
            Assert.That(list.get(0), Is.EqualTo("Mr. M"));
            Assert.That(list.get(1), Is.EqualTo("Mr. B"));

            Assert.That(value.Authors.Count, Is.EqualTo(2));
            Assert.That(value.Authors[0], Is.EqualTo("Mr. M"));
            Assert.That(value.Authors[1], Is.EqualTo("Mr. B"));
        }

        [Test]
        public void TestListWithObjects()
        {
            var dotNetTypeLookup = Initialize();
            var value = new DotNetTests.TestClassWithList();
            var dotNetElement = dotNetTypeLookup.CreateDotNetElement(value);

            var persons = dotNetElement.get("Persons");
            Assert.That(persons, Is.Not.Null);
            Assert.That(persons, Is.InstanceOf<IReflectiveSequence>());

            var list = persons as IReflectiveSequence;

            Assert.That(list, Is.Not.Null);
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(list.size(), Is.EqualTo(0));

            var person1 = new DotNetTests.Person {Name = "M"};

            list.add(person1);
            Assert.That(list.size(), Is.EqualTo(1));

            var personGot = list.get(0);
            Assert.That(personGot, Is.Not.Null);
            Assert.That(personGot, Is.InstanceOf<IElement>());

            var personAsElement = personGot as IElement;
            // ReSharper disable once PossibleNullReferenceException
            Assert.That(personAsElement.metaclass.get("name"), Contains.Substring("Person"));

            Assert.That(personAsElement.get("Name"), Is.EqualTo("M"));
            personAsElement.set("Prename", "Herr");
            Assert.That(person1.Prename, Is.EqualTo("Herr"));
        }

        [Test]
        public void TestExtent()
        {
            var lookup = Initialize();
            var extent = new DotNetExtent("dm:///test", lookup);
            Assert.That(extent.elements(), Is.Not.Null);
            extent.elements().add("Test");
            extent.elements().add(new DotNetTests.Person());

            Assert.That(extent.elements().size(), Is.EqualTo(2));
        }

        [Test]
        public void TestFactory()
        {
            var lookup = Initialize();
            var metaClass = lookup.ToElement(typeof(DotNetTests.TestClass));
            var factory = new DotNetFactory(null, lookup);
            var created = factory.create(metaClass);
            Assert.That(created, Is.TypeOf<DotNetElement>());
            created.set("Title", "Test");
            Assert.That(created.getMetaClass(), Is.EqualTo(metaClass));
            Assert.That(created.get("Title"), Is.EqualTo("Test"));

            var found = (DotNetElement) created;
            Assert.That(((DotNetTests.TestClass) found.GetNativeValue()).Title, Is.EqualTo("Test"));
        }

        [Test]
        public static void TestExtentReferences()
        {
            var lookup = Initialize();
            var extent = new DotNetExtent("dm:///test", lookup);

            var parent = new DotNetTests.PersonWithParent
            {
                Name = "Eric",
                Prename = "Maier"
            };

            var child = new DotNetTests.PersonWithParent
            {
                Name = "Ericson",
                Prename = "Maierson",
                Parent = parent
            };

            var personAsElement = lookup.CreateDotNetElement(child);
            extent.elements().add(personAsElement);

            var element = extent.elements().ElementAt(0).AsIObject();
            Assert.That(element != null);
            var asKnowsExtent = (IHasExtent) element;
            Assert.That(asKnowsExtent.Extent, Is.EqualTo(extent));

            var parentObject = element.get("Parent");
            Assert.That(parentObject, Is.Not.Null);
            var parentAsObject = parentObject.AsIObject();
            Assert.That(parentAsObject, Is.Not.Null);
            asKnowsExtent = (IHasExtent)parentAsObject;
            Assert.That(asKnowsExtent.Extent, Is.EqualTo(extent));
        }

        [Test]
        public static void TestExtentReferencesWithSequence()
        {
            var lookup = Initialize();
            var extent = new DotNetExtent("dm:///test", lookup);

            var parent = new DotNetTests.TestClassWithList
            {
                Title = "TestClass"
            };

            var child1 = new DotNetTests.Person
            {
                Name = "Ericson",
                Prename = "Maierson"
            };

            var child2 = new DotNetTests.Person
            {
                Name = "Ericson",
                Prename = "Maierson"
            };

            parent.Persons.Add(child1);
            parent.Persons.Add(child2);

            var personAsElement = lookup.CreateDotNetElement(parent);
            extent.elements().add(personAsElement);

            var element = extent.elements().ElementAt(0).AsIObject();

            // Gets the children and verifies if they are a reflective colleciton
            var parentObject = element.get("Persons");
            Assert.That(parentObject, Is.Not.Null);
            var persons = (IReflectiveCollection) parentObject;
            Assert.That(persons, Is.Not.Null);

            // Ok, now get the child and see, if OK
            var childRetrieved = persons.ElementAt(0);
            Assert.That(childRetrieved, Is.Not.Null);
            var asKnowsExtent = (IHasExtent) childRetrieved;
            Assert.That(asKnowsExtent.Extent, Is.EqualTo(extent));
        }

        private static DotNetTypeLookup Initialize()
        {
            var uml = new _UML();
            var mofFactory = new InMemoryFactory();
            var dotNetTypeLookup = new DotNetTypeLookup();
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.TestClass));
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.Person));
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.PersonWithParent));
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.TestClassWithList));
            return dotNetTypeLookup;
        }

    }
}