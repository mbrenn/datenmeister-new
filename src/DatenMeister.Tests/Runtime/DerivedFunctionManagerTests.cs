using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime
{
    [TestFixture]
    public class DerivedFunctionManagerTests
    {
        [Test]
        public void TestDerivedMethods()
        {
            var typeWorkspace = new Workspace("Types");

            var typeExtent = new MofUriExtent(new InMemoryProvider());
            
            var typeFactory = new MofFactory(typeExtent);

            // Creates the class
            var classMethod = typeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            var property1Method = typeFactory.create(_UML.TheOne.Classification.__Property);
            property1Method.set(_UML._CommonStructure._NamedElement.name, "age");
            var property2Method = typeFactory.create(_UML.TheOne.Classification.__Property);
            property2Method.set(_UML._CommonStructure._NamedElement.name, "doubleage");

            classMethod.set(
                _UML._StructuredClassifiers._Class.ownedAttribute, 
                new []{property1Method, property2Method});
            
            typeExtent.elements().add(classMethod);
            typeWorkspace.AddExtent(typeExtent);
            
            // Create the derived method
            typeWorkspace.DynamicFunctionManager.AddDerivedProperty(
                classMethod,
                "doubleage", o => o.getOrDefault<int>("age") * 2);
            
            // The item to be created
            var itemExtent = new MofUriExtent(new InMemoryProvider());
            var itemFactory = new MofFactory(itemExtent);

            var itemFound = itemFactory.create(classMethod);
            itemFound.set("age", 18);
            
            // Now the checks
            var metaClass = itemFound.metaclass!;
            Assert.That(metaClass, Is.Not.Null);
            Assert.That(metaClass.Equals(classMethod));
            Assert.That(itemFound.isSet("age"), Is.True);
            Assert.That(itemFound.isSet("doubleage"), Is.True);

            Assert.That(itemFound.getOrDefault<int>("age"), Is.EqualTo(18));
            Assert.That(itemFound.getOrDefault<int>("doubleage"), Is.EqualTo(36));
        }
    }
}