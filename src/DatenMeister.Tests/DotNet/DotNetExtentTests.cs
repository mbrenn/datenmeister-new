
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
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
            var provider = new DotNetProvider(dotNetTypeLookup);
            var value = new DotNetTests.TestClass();
            var dotNetElement = dotNetTypeLookup.CreateDotNetElement(provider, value);

            Assert.That(dotNetElement.GetProperty("Title"), Is.Null);
            Assert.That(dotNetElement.GetProperty("Number"), Is.Not.Null);
            Assert.That(dotNetElement.GetProperty("Number"), Is.EqualTo(0));

            dotNetElement.SetProperty("Title", "Your Movie");
            dotNetElement.SetProperty("Number", 3);

            Assert.That(value.Title, Is.EqualTo("Your Movie"));
            Assert.That(value.Number, Is.EqualTo(3));
        }

        [Test]
        public void TestListWithStrings()
        {
            var dotNetTypeLookup = Initialize();
            var provider = new DotNetProvider(dotNetTypeLookup);
            var value = new DotNetTests.TestClassWithList();
            var dotNetElement = dotNetTypeLookup.CreateDotNetElement(provider, value);

            var result = dotNetElement.GetProperty("Authors");
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
            var provider = new DotNetProvider(dotNetTypeLookup);
            var dotNetElement = dotNetTypeLookup.CreateDotNetElement(provider, value);

            var persons = dotNetElement.GetProperty("Persons");
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
            var provider = new DotNetProvider(lookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            Assert.That(extent.elements(), Is.Not.Null);
            extent.elements().add(new DotNetTests.Person());

            Assert.That(extent.elements().size(), Is.EqualTo(1));
        }

        [Test]
        public void TestFactory()
        {
            var lookup = Initialize();
            var provider = new DotNetProvider(lookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            var factory = new MofFactory(extent);

            var metaClass = lookup.ToElement(typeof(DotNetTests.TestClass));
            var created = factory.create(metaClass);
            Assert.That(created, Is.TypeOf<MofElement>());
            created.set("Title", "Test");
            Assert.That(created.getMetaClass(), Is.EqualTo(metaClass));
            Assert.That(created.get("Title"), Is.EqualTo("Test"));

            var found = (DotNetProviderObject) ((MofElement)created).ProviderObject;
            Assert.That(((DotNetTests.TestClass)found.GetNativeValue()).Title, Is.EqualTo("Test"));
        }

        [Test]
        public static void TestExtentReferences()
        {
            var lookup = Initialize();
            var provider = new DotNetProvider(lookup);
            var extent = new MofUriExtent(provider, "dm:///test");

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

            var personAsElement = lookup.CreateDotNetElement(provider, child);
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
            var provider = new DotNetProvider(lookup);
            var extent = new MofUriExtent(provider, "dm:///test");

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

            var personAsElement = lookup.CreateDotNetElement(provider, parent);
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

        internal static DotNetTypeLookup Initialize()
        {
            var uml = new _UML();
            var dotNetTypeLookup = new DotNetTypeLookup();
            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var factory = new MofFactory(extent);

            dotNetTypeLookup.GenerateAndAdd(uml, extent, typeof(DotNetTests.TestClass));
            dotNetTypeLookup.GenerateAndAdd(uml, extent, typeof(DotNetTests.Person));
            dotNetTypeLookup.GenerateAndAdd(uml, extent, typeof(DotNetTests.PersonWithParent));
            dotNetTypeLookup.GenerateAndAdd(uml, extent, typeof(DotNetTests.TestClassWithList));
            return dotNetTypeLookup;
        }

    }
}