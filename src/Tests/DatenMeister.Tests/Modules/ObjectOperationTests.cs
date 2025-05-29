using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime;
using DatenMeister.Modules.DefaultTypes;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules;

[TestFixture]
public class ObjectOperationTests
{
    [Test]
    public void TestAddNewItemAsReferenceToInstanceProperty()
    {
        //
        // Prepare all the stuff that is necessary
        var provider = new XmiProvider();
        var dataExtent = new MofUriExtent(provider, null);
        var dataFactory = new MofFactory(dataExtent);

        var typeProvider = new InMemoryProvider();
        var typeExtent = new MofUriExtent(typeProvider, null);

        var typeFactory = new MofFactory(typeExtent);
        var class1 = typeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
        class1.set(_UML._StructuredClassifiers._Class.name, "Name");

        var property1 = typeFactory.create(_UML.TheOne.Classification.__Property);
        property1.set(_UML._Classification._Property.name, "name");
        var property2 = typeFactory.create(_UML.TheOne.Classification.__Property);
        property2.set(_UML._Classification._Property.name, "child");
        property2.set(_UML._Classification._Property.isComposite, true);
        var property3 = typeFactory.create(_UML.TheOne.Classification.__Property);
        property3.set(_UML._Classification._Property.name, "husband");

        class1.set(_UML._StructuredClassifiers._Class.ownedAttribute,
            new[] {property1, property2, property3});

        typeExtent.elements().add(class1);

        var classPackage = typeFactory.create(_UML.TheOne.StructuredClassifiers.__Class);
        classPackage.set(_UML._StructuredClassifiers._Class.name, "Name");

        var property4 = typeFactory.create(_UML.TheOne.Classification.__Property);
        property4.set(_UML._Classification._Property.name, "packagedElement");

        classPackage.set(_UML._StructuredClassifiers._Class.ownedAttribute,
            new[] {property4});

        typeExtent.elements().add(classPackage);

        dataExtent.AddMetaExtent(typeExtent);

        // 
        // Create the data
        var instanceRoot = dataFactory.create(class1);
        instanceRoot.set("name", "root");
        var instancePackage = dataFactory.create(classPackage);
        instancePackage.set("name", "package");
        var instanceInPackage = dataFactory.create(class1);
        instanceInPackage.set("name", "inpackage");

        dataExtent.elements().add(instanceRoot);
        dataExtent.elements().add(instancePackage);

        DefaultClassifierHints.AddToExtentOrElement(instancePackage, instanceInPackage);


        // Now perform the tests
        var child1 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
            instanceRoot,
            "child",
            class1,
            dataExtent,
            false);
        child1.set("name", "child root");

        var child2 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
            instanceRoot,
            "husband",
            class1,
            dataExtent,
            false);
        child2.set("name", "husband root");

        var child3 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
            instanceInPackage,
            "child",
            class1,
            instancePackage,
            false);
        child3.set("name", "child package");

        var child4 = ObjectOperations.AddNewItemAsReferenceToInstanceProperty(
            instanceInPackage,
            "husband",
            class1,
            instancePackage,
            false);
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
        var husbandRoot = elements.FirstOrDefault(x => x.getOrDefault<string>("name") == "husband root");
        Assert.That(husbandRoot, Is.Not.Null);

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
        Assert.That(packageChildren.FirstOrDefault(x => x.getOrDefault<string>("name") == "husband package"), Is.Not.Null);


        Assert.That(
            copyInPackage.getOrDefault<IElement>("child").getOrDefault<string>("name"),
            Is.EqualTo("child package"));
        Assert.That(
            copyInPackage.getOrDefault<IElement>("husband").getOrDefault<string>("name"),
            Is.EqualTo("husband package"));
    }
}