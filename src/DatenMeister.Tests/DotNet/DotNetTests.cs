﻿using System.Collections.Generic;
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
    }
}