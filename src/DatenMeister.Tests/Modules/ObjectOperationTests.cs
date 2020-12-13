using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class ObjectOperationTests
    {
        [Test]
        public void TestAddNewItemAsReferenceToInstanceProperty()
        {
            var (dataExtent, dataFactory, classPerson, classPackage) 
                = GetDataAndTypeExtent();

            var (instanceRoot, instancePackage, instanceInPackage)
                = PrepareData(dataFactory, classPerson, classPackage, dataExtent);
            
            // Now perform the tests
            var child1 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
                instanceRoot,
                "child",
                classPerson,
                dataExtent);
            child1.set("name", "child root");

            var child2 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
                instanceRoot,
                "husband",
                classPerson,
                dataExtent);
            child2.set("name", "husband root");

            var child3 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
                instanceInPackage,
                "child",
                classPerson,
                instancePackage);
            child3.set("name", "child package");

            var child4 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
                instanceInPackage,
                "husband",
                classPerson,
                instancePackage);
            child4.set("name", "husband package");

            Assert.That(child1, Is.Not.Null);
            Assert.That(child2, Is.Not.Null);
            Assert.That(child3, Is.Not.Null);
            Assert.That(child4, Is.Not.Null);

            var elements = dataExtent.elements().OfType<IElement>().ToList();
            Assert.That(elements.Count, Is.EqualTo(3));
            Assert.That(elements.Any(x => x.getOrDefault<string>("name") == "root"), Is.True);
            Assert.That(elements.Any(x => x.getOrDefault<string>("name") == "package"), Is.True);
            Assert.That(elements.Any(x => x.getOrDefault<string>("name") == "husband root"), Is.True);

            var copyRoot =
                elements.FirstOrDefault(x => x.getOrDefault<string>("name") == "root");
            var copyPackage = 
                elements.FirstOrDefault(x => x.getOrDefault<string>("name") == "package");
            var copyChildRoot =
                elements.FirstOrDefault(x => x.getOrDefault<string>("name") == "husband root");

            Assert.That(
                copyRoot.getOrDefault<IElement>("child").getOrDefault<string>("name"), 
                Is.EqualTo("child root"));
            Assert.That(
                copyRoot.getOrDefault<IElement>("husband").getOrDefault<string>("name"),
                Is.EqualTo("husband root"));

            var packageChildren =
                copyPackage.getOrDefault<IReflectiveCollection>("packagedElement")
                    .OfType<IElement>().ToList();

            Assert.That(packageChildren.Count, Is.EqualTo(2));
            Assert.That(packageChildren.Any(x => x.getOrDefault<string>("name") == "inpackage"), Is.True);
            Assert.That(packageChildren.Any(x => x.getOrDefault<string>("name") == "husband package"), Is.True);

            var copyInPackage =
                packageChildren.FirstOrDefault(x => x.getOrDefault<string>("name") == "inpackage");
            var copyHusbandPackage =
                packageChildren.FirstOrDefault(x => x.getOrDefault<string>("name") == "husband package");


            Assert.That(
                copyInPackage.getOrDefault<IElement>("child").getOrDefault<string>("name"),
                Is.EqualTo("child package"));
            Assert.That(
                copyInPackage.getOrDefault<IElement>("husband").getOrDefault<string>("name"),
                Is.EqualTo("husband package"));
        }

        [Test]
        public void TestAddNewItemAsReferenceCollectionToInstanceProperty()
        {
            var (dataExtent, dataFactory, classPerson, classPackage)
                = GetDataAndTypeExtent();

            var (instanceRoot, instancePackage, instanceInPackage)
                = PrepareData(dataFactory, classPerson, classPackage, dataExtent);

            // Now perform the tests
            var child1 = ObjectOperations.AddNewItemAsReferenceAsCollectionToInstanceProperty(
                instanceRoot,
                "child",
                classPerson,
                dataExtent);
            child1.set("name", "child root");

            // Now perform the tests
            var child2 = ObjectOperations.AddNewItemAsReferenceAsCollectionToInstanceProperty(
                instanceRoot,
                "child",
                classPerson,
                dataExtent);
            child2.set("name", "child root 2");


            var child3 = ObjectOperations.AddNewItemAsReferenceAsCollectionToInstanceProperty(
                instanceInPackage,
                "child",
                classPerson,
                instancePackage);
            child3.set("name", "child package");

            var child4 = ObjectOperations.AddNewItemAsReferenceAsCollectionToInstanceProperty(
                instanceInPackage,
                "child",
                classPerson,
                instancePackage);
            child4.set("name", "child package 2");

            Assert.That(child1, Is.Not.Null);
            Assert.That(child2, Is.Not.Null);
            Assert.That(child3, Is.Not.Null);
            Assert.That(child4, Is.Not.Null);

            var elements = dataExtent.elements().OfType<IElement>().ToList();
            Assert.That(elements.Count, Is.EqualTo(2));
            Assert.That(elements.Any(x => x.getOrDefault<string>("name") == "root"), Is.True);
            Assert.That(elements.Any(x => x.getOrDefault<string>("name") == "package"), Is.True);

            var copyRoot =
                elements.FirstOrDefault(x => x.getOrDefault<string>("name") == "root");
            var copyPackage =
                elements.FirstOrDefault(x => x.getOrDefault<string>("name") == "package");

            var childElements = copyRoot.getOrDefault<IReflectiveCollection>("child")
                .OfType<IElement>().ToList();
            Assert.That(childElements.Count, Is.EqualTo(2));

            Assert.That(childElements.Any(x => x.getOrDefault<string>("name") == "child root"), Is.True);
            Assert.That(childElements.Any(x => x.getOrDefault<string>("name") == "child root 2"), Is.True);

            var packagedElement = copyPackage.getOrDefault<IElement>("packagedElement");
            Assert.That(packagedElement, Is.Not.Null);
            
            var childElements2 = packagedElement.getOrDefault<IReflectiveCollection>("child")
                .OfType<IElement>().ToList();
            Assert.That(childElements2.Count, Is.EqualTo(2));

            Assert.That(childElements2.Any(x => x.getOrDefault<string>("name") == "child package"), Is.True);
            Assert.That(childElements2.Any(x => x.getOrDefault<string>("name") == "child package 2"), Is.True);
        }


        private static (IElement instanceRoot, IElement instancePackage, IElement instanceInPackage) PrepareData(
            MofFactory dataFactory, IElement classPerson, IElement classPackage, MofUriExtent dataExtent)
        {
            // 
            // Create the data
            var instanceRoot = dataFactory.create(classPerson);
            instanceRoot.set("name", "root");
            var instancePackage = dataFactory.create(classPackage);
            instancePackage.set("name", "package");
            var instanceInPackage = dataFactory.create(classPerson);
            instanceInPackage.set("name", "inpackage");

            dataExtent.elements().add(instanceRoot);
            dataExtent.elements().add(instancePackage);

            DefaultClassifierHints.AddToExtentOrElement(instancePackage, instanceInPackage);
            return (instanceRoot, instancePackage, instanceInPackage);
        }

        private static
            (MofUriExtent dataExtent, MofFactory dataFactory, IElement classPerson, IElement classPackage)
            GetDataAndTypeExtent()
        {
            //
            // Prepare all the stuff that is necessary
            var provider = new XmiProvider();
            var dataExtent = new MofUriExtent(provider);
            var dataFactory = new MofFactory(dataExtent);

            var typeProvider = new InMemoryProvider();
            var typeExtent = new MofUriExtent(typeProvider);

            var typeFactory = new MofFactory(typeExtent);
            var classPerson = typeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            classPerson.set(_UML._StructuredClassifiers._Class.name, "Name");

            var property1 = typeFactory.create(_UML.TheOne.Classification.__Property);
            property1.set(_UML._Classification._Property.name, "name");
            var property2 = typeFactory.create(_UML.TheOne.Classification.__Property);
            property2.set(_UML._Classification._Property.name, "child");
            property2.set(_UML._Classification._Property.isComposite, true);
            var property3 = typeFactory.create(_UML.TheOne.Classification.__Property);
            property3.set(_UML._Classification._Property.name, "husband");

            classPerson.set(_UML._StructuredClassifiers._Class.ownedAttribute,
                new[] {property1, property2, property3});

            typeExtent.elements().add(classPerson);

            var classPackage = typeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
            classPackage.set(_UML._StructuredClassifiers._Class.name, "Name");

            var property4 = typeFactory.create(_UML.TheOne.Classification.__Property);
            property4.set(_UML._Classification._Property.name, "packagedElement");

            classPackage.set(_UML._StructuredClassifiers._Class.ownedAttribute,
                new[] {property4});

            typeExtent.elements().add(classPackage);

            dataExtent.AddMetaExtent(typeExtent);
            return (dataExtent, dataFactory, classPerson, classPackage);
        }
    }
}