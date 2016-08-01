using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
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

        private static DotNetTypeLookup Initialize()
        {
            var uml = new _UML();
            var mofFactory = new MofFactory();
            var dotNetTypeLookup = new DotNetTypeLookup();
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.TestClass));
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.Person));
            dotNetTypeLookup.GenerateAndAdd(uml, mofFactory, typeof(DotNetTests.TestClassWithList));
            return dotNetTypeLookup;
        }
    }
}