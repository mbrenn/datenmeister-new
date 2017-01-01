using System.Collections.Generic;
using System.Linq;
using DatenMeister.Apps.ZipCode.Model;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Tests.Xmi;
using NUnit.Framework;

namespace DatenMeister.Tests.ZipCodes
{
    public class ZipCodeTests
    {
        [Test]
        public void TestDotNetTypeCreation()
        {
            _MOF mof;
            _UML uml;
            XmiTests.CreateUmlAndMofInstance(out mof, out uml);

            var mofFactory = (IFactory) null; // new InMemoryFactory();
            var dotNetTypeCreator = new DotNetTypeGenerator(mofFactory, uml);
            var dotNetClass = dotNetTypeCreator.CreateTypeFor(typeof(ZipCode));

            Assert.That(dotNetClass.get(_UML._CommonStructure._NamedElement.name), Is.EqualTo("ZipCode"));

            var ownedAttributes = dotNetClass.get(_UML._Classification._Classifier.attribute) as IEnumerable<object>;
            Assert.That(ownedAttributes, Is.Not.Null);

            var asList = ownedAttributes.ToList();
            Assert.That(asList.Count, Is.GreaterThanOrEqualTo(5));

            Assert.That(asList[0] as IElement, Is.Not.Null);
            var firstProperty = (asList[0] as IElement).get(_UML._CommonStructure._NamedElement.name);
            Assert.That(firstProperty, Is.Not.Null);
            Assert.That(firstProperty.ToString(), Is.EqualTo("Id"));

            Assert.That(asList[1] as IElement, Is.Not.Null);
            var secondProperty = (asList[1] as IElement).get(_UML._CommonStructure._NamedElement.name);
            Assert.That(secondProperty, Is.Not.Null);
            Assert.That(secondProperty.ToString(), Is.EqualTo("Zip"));

        }
    }
}