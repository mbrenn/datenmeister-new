using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class DotNetExtentTests
    {
        [Test]
        public void TestValue()
        {
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            var value = new DotNetTests.TestClass();
            var dotNetElement = extent.CreateDotNetMofElement(value);

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
            var extent = Initialize();
            var provider = new DotNetProvider(extent.TypeLookup);
            var dotNetExtent = new MofUriExtent(provider, "dm:///");
            var value = new DotNetTests.TestClassWithList();
            var dotNetElement = dotNetExtent.CreateDotNetMofElement(value);

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
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            extent.AddMetaExtent(typeExtent);
            var value = new DotNetTests.TestClassWithList();
            var dotNetElement = extent.CreateDotNetMofElement(value);

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
            Assert.That(personAsElement.get("Prename"), Is.EqualTo("Herr"));
        }

        [Test]
        public void TestExtent()
        {
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            extent.AddMetaExtent(typeExtent);
            Assert.That(extent.elements(), Is.Not.Null);
            extent.elements().add(new DotNetTests.Person());

            Assert.That(extent.elements().size(), Is.EqualTo(1));
        }

        [Test]
        public void TestFactory()
        {
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            var factory = new MofFactory(extent);

            var metaClass = typeExtent.TypeLookup.ToElement(typeof(DotNetTests.TestClass));
            var created = factory.create(typeExtent.ResolveElement(metaClass, ResolveType.Default));
            Assert.That(created, Is.TypeOf<MofElement>());
            created.set("Title", "Test");
            var metaClassObject = created.getMetaClass();
            Assert.That(metaClassObject, Is.Not.Null);
            Assert.That(metaClassObject.GetUri(), Is.EqualTo(metaClass));
            Assert.That(created.get("Title"), Is.EqualTo("Test"));

            var found = (DotNetProviderObject) ((MofElement)created).ProviderObject;
            Assert.That(((DotNetTests.TestClass)found.GetNativeValue()).Title, Is.EqualTo("Test"));
        }

        [Test]
        public static void TestExtentReferences()
        {
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
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

            var personAsElement = extent.CreateDotNetMofElement(child);
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
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
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

            var personAsElement = extent.CreateDotNetMofElement(parent);
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

        [Test]
        public void TestWithElementsWithinSameExtent()
        {
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            
            // Creates the spouse and the children and adds them to the extent
            var spouse = new DotNetTests.Person
            {
                Name = "Spouse",
                Prename = "Maierwoman"
            };

            var child1 = new DotNetTests.Person
            {
                Name = "Child1",
                Prename = "Maierson"
            };
            
            var child2 = new DotNetTests.Person
            {
                Name = "Child2",
                Prename = "Maierdaughter"
            };

            extent.elements().add(extent.CreateDotNetMofElement(spouse));
            extent.elements().add(extent.CreateDotNetMofElement(child1));
            extent.elements().add(extent.CreateDotNetMofElement(child2));

            // Checks that children and spouse are properly added
            var spouseElement = extent.elements().WhenPropertyHasValue("Name", "Spouse").FirstOrDefault() as IElement;
            var child1Element = extent.elements().WhenPropertyHasValue("Name", "Child1").FirstOrDefault() as IElement;
            var child2Element = extent.elements().WhenPropertyHasValue("Name", "Child2").FirstOrDefault() as IElement;

            Assert.That(spouseElement, Is.Not.Null);
            Assert.That(child1Element, Is.Not.Null);
            Assert.That(spouseElement, Is.Not.Null);
            
            // Now creates the element referencing spouse and children via IElement
            var nonSpouse = new DotNetTests.PersonWithAnotherPersonPerElement
            {
                Name = "Husband",
                Spouse = spouseElement,
                Children = new List<IElement> {child1Element, child2Element}
            };

            extent.elements().add(extent.CreateDotNetMofElement(nonSpouse));

            var nonSpouseElement = extent.elements().WhenPropertyHasValue("Name", "Husband").FirstOrDefault() as IElement;
            Assert.That(nonSpouseElement, Is.Not.Null);
            Assert.That(nonSpouseElement.getOrDefault<IElement>("Spouse"), Is.Not.Null);
            Assert.That(nonSpouseElement.getOrDefault<IElement>("Spouse").getOrDefault<string>("Name"),
                Is.EqualTo("Spouse"));

            var children = nonSpouseElement.getOrDefault<IReflectiveCollection>("Children");
            Assert.That(children, Is.Not.Null);
            Assert.That((children.ElementAt(0) as IElement).getOrDefault<string>("Name"), Is.EqualTo("Child1"));
            Assert.That((children.ElementAt(1) as IElement).getOrDefault<string>("Name"), Is.EqualTo("Child2"));
        }

        [Test]
        public void TestWithElementsInOtherExtent()
        {
            var typeExtent = Initialize();
            var provider = new DotNetProvider(typeExtent.TypeLookup);
            var extent = new MofUriExtent(provider, "dm:///test");
            var otherExtent = new MofUriExtent(new InMemoryProvider(), "dm:///otherextent");
            var workspace = new Workspace("Data");
            workspace.AddExtent(otherExtent);
            workspace.AddExtent(extent);
            
            // Creates the spouse and the children and adds them to the extent
            var spouse = new DotNetTests.Person
            {
                Name = "Spouse",
                Prename = "Maierwoman"
            };

            var child1 = new DotNetTests.Person
            {
                Name = "Child1",
                Prename = "Maierson"
            };
            
            var child2 = new DotNetTests.Person
            {
                Name = "Child2",
                Prename = "Maierdaughter"
            };

            otherExtent.elements().add(DotNetConverter.ConvertToMofObject(otherExtent, spouse) ??
                                       throw new InvalidOperationException("Should not be null"));
            otherExtent.elements().add(DotNetConverter.ConvertToMofObject(otherExtent, child1) ??
                                       throw new InvalidOperationException("Should not be null"));
            otherExtent.elements().add(DotNetConverter.ConvertToMofObject(otherExtent, child2) ??
                                       throw new InvalidOperationException("Should not be null"));

            // Checks that children and spouse are properly added
            var spouseElement = otherExtent.elements().WhenPropertyHasValue("Name", "Spouse").FirstOrDefault() as IElement;
            var child1Element = otherExtent.elements().WhenPropertyHasValue("Name", "Child1").FirstOrDefault() as IElement;
            var child2Element = otherExtent.elements().WhenPropertyHasValue("Name", "Child2").FirstOrDefault() as IElement;

            Assert.That(spouseElement, Is.Not.Null);
            Assert.That(child1Element, Is.Not.Null);
            Assert.That(spouseElement, Is.Not.Null);
            
            // Now creates the element referencing spouse and children via IElement
            var nonSpouse = new DotNetTests.PersonWithAnotherPersonPerElement
            {
                Name = "Husband",
                Spouse = spouseElement,
                Children = new List<IElement> {child1Element, child2Element}
            };

            extent.elements().add(extent.CreateDotNetMofElement(nonSpouse));
            

            var nonSpouseElement = extent.elements().WhenPropertyHasValue("Name", "Husband").FirstOrDefault() as IElement;
            Assert.That(nonSpouseElement, Is.Not.Null);
            Assert.That(nonSpouseElement.getOrDefault<IElement>("Spouse"), Is.Not.Null);
            Assert.That(nonSpouseElement.getOrDefault<IElement>("Spouse").getOrDefault<string>("Name"),
                Is.EqualTo("Spouse"));

            var children = nonSpouseElement.getOrDefault<IReflectiveCollection>("Children");
            Assert.That(children, Is.Not.Null);
            Assert.That((children.ElementAt(0) as IElement).getOrDefault<string>("Name"), Is.EqualTo("Child1"));
            Assert.That((children.ElementAt(1) as IElement).getOrDefault<string>("Name"), Is.EqualTo("Child2"));
            
            // Now the challange... change the content in the other extent and it should be reflected here. 
            spouseElement.set("Name", "New Spouse");
            child1Element.set("Name", "New Child");
            
            Assert.That(nonSpouseElement.getOrDefault<IElement>("Spouse").getOrDefault<string>("Name"),
                Is.EqualTo("New Spouse"));

            children = nonSpouseElement.getOrDefault<IReflectiveCollection>("Children");
            Assert.That((children.ElementAt(0) as IElement).getOrDefault<string>("Name"), Is.EqualTo("New Child"));
        }

        internal static MofUriExtent Initialize()
        {
            var uml = new _UML();
            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test");

            extent.CreateTypeSpecification(typeof(DotNetTests.TestClass));
            extent.CreateTypeSpecification(typeof(DotNetTests.Person));
            extent.CreateTypeSpecification(typeof(DotNetTests.PersonWithParent));
            extent.CreateTypeSpecification(typeof(DotNetTests.TestClassWithList));
            extent.CreateTypeSpecification(typeof(DotNetTests.PersonWithAnotherPersonPerElement));
            return extent;
        }

    }
}