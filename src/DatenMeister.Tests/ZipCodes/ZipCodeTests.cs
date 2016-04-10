using DatenMeister.Apps.ZipCode.Model;
using DatenMeister.EMOF.InMemory;
using DatenMeister.Provider.DotNet;
using DatenMeister.Tests.Xmi;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Reflection;

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

            var mofFactory = new MofFactory();
            var dotNetTypeCreator = new DotNetTypeGenerator(mofFactory, uml);
            var dotNetClass = dotNetTypeCreator.CreateTypeFor(typeof(ZipCode));

            Assert.That(dotNetClass.get(uml.CommonStructure.NamedElement.name), Is.EqualTo("ZipCode"));

            var ownedAttributes = dotNetClass.get(uml.Classification.Classifier.attribute) as IEnumerable<object>;
            Assert.That(ownedAttributes, Is.Not.Null);

            var asList = ownedAttributes.ToList();
            Assert.That(asList.Count, Is.GreaterThanOrEqualTo(5));

            Assert.That(asList[0] as IElement, Is.Not.Null);
            var firstProperty = (asList[0] as IElement).get(uml.CommonStructure.NamedElement.name);
            Assert.That(firstProperty, Is.Not.Null);
            Assert.That(firstProperty.ToString(), Is.EqualTo("Id"));

            Assert.That(asList[1] as IElement, Is.Not.Null);
            var secondProperty = (asList[1] as IElement).get(uml.CommonStructure.NamedElement.name);
            Assert.That(secondProperty, Is.Not.Null);
            Assert.That(secondProperty.ToString(), Is.EqualTo("Zip"));

        }
    }
}