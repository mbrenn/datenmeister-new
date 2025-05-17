using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager;
using NUnit.Framework;
using _PrimitiveTypes = DatenMeister.Core.Models.EMOF._PrimitiveTypes;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestClassifierMethods
    {
        [Test]
        public async Task TestCompositeProperties()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var classifier = dm.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_UML.TheOne.Classification.__Classifier.GetUri()!, ResolveType.Default)
                as IElement;
            Assert.That(classifier, Is.Not.Null);

            var properties = ClassifierMethods.GetCompositingProperties(classifier!).ToList();
            Assert.That(properties, Is.Not.Null);
            var propertyList = properties.ToList();
            Assert.That(propertyList.Count(), Is.GreaterThan(0));

            var allProperties = ClassifierMethods.GetPropertiesOfClassifier(classifier!).ToList();
            Assert.That(allProperties, Is.Not.Null);
            var allPropertyList = allProperties.ToList();
            Assert.That(allPropertyList.Count, Is.GreaterThan(propertyList.Count));
        }

        [Test]
        public async Task TestAddGeneralization()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var classifier = dm.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_UML.TheOne.Classification.__Classifier.GetUri()!, ResolveType.Default)
                as IElement;
            var extent = XmiExtensions.CreateXmiExtent("dm:///test");
            var factory = new MofFactory(extent);
            var classSpecialized = factory.create(classifier);
            var classGeneralized = factory.create(classifier);

            extent.elements().add(classSpecialized);
            extent.elements().add(classGeneralized);

            ClassifierMethods.AddGeneralization(
                classSpecialized,
                classGeneralized);

            var generalizations = ClassifierMethods.GetGeneralizations(classGeneralized).ToList();
            Assert.That(generalizations.Count, Is.EqualTo(0));

            generalizations = ClassifierMethods.GetGeneralizations(classSpecialized).ToList();
            Assert.That(generalizations.Count, Is.EqualTo(1));
            Assert.That(generalizations.First().Equals(classGeneralized), Is.True);
        }

        [Test]
        public async Task TestGeneralizedProperties()
        {
            await using var builder = await DatenMeisterTests.GetDatenMeisterScope();

            // Gets the logic
            var feature = builder.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_UML.TheOne.Classification.__Feature.GetUri()!, ResolveType.Default)
                as IElement;
            var properties = ClassifierMethods.GetPropertyNamesOfClassifier(feature!).ToList();

            Assert.That(properties.Contains(_UML._Classification._Feature.isStatic), Is.True,
                "isStatic");
            Assert.That(properties.Contains(_UML._Classification._RedefinableElement.isLeaf), Is.True,
                "isLeaf (Parent)");
            Assert.That(properties.Contains(_UML._CommonStructure._NamedElement.name), Is.True,
                "name (Parent of Parent)");
        }

        [Test]
        public async Task TestGeneralizationEvaluation()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

            var classifier = dm.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_UML.TheOne.Classification.__Classifier.GetUri()!, ResolveType.Default)
                as IElement;
            var class2 = dm.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_UML.TheOne.StructuredClassifiers.__Class.GetUri()!, ResolveType.Default)
                as IElement;
            var comment = dm.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_UML.TheOne.CommonStructure.__Comment.GetUri()!, ResolveType.Default)
                as IElement;

            var isSpecialized = ClassifierMethods.IsSpecializedClassifierOf(
                class2,
                classifier);
            Assert.That(isSpecialized, Is.True);

            isSpecialized = ClassifierMethods.IsSpecializedClassifierOf(
                comment,
                classifier);
            Assert.That(isSpecialized, Is.False);
        }

        [Test]
        public async Task TestIsOfPrimitiveType()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();

            var activity = dm.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_UML.TheOne.Activities.__Activity.GetUri()!, ResolveType.Default)
                as IElement;
            var integer = dm.WorkspaceLogic.GetUmlWorkspace()
                    .Resolve(_PrimitiveTypes.TheOne.__Integer.GetUri()!, ResolveType.Default)
                as IElement;

            Assert.That(ClassifierMethods.IsOfPrimitiveType(activity!), Is.False);
            Assert.That(ClassifierMethods.IsOfPrimitiveType(integer!), Is.True);
        }

        [Test]
        public async Task TestGetPropertyTypeOfMetaClass()
        {
            await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var commandLineMetaClass = dm.WorkspaceLogic.GetTypesWorkspace()
                    .Resolve(_DatenMeister.TheOne.CommonTypes.OSIntegration.__CommandLineApplication.GetUri()!,
                        ResolveType.Default)
                as IElement;
            
            Assert.That(commandLineMetaClass, Is.Not.Null);

            var foundPropertyType = ClassifierMethods.GetPropertyType(commandLineMetaClass,
                _DatenMeister._CommonTypes._OSIntegration._CommandLineApplication.name);
            Assert.That(foundPropertyType, Is.Not.Null);
            Assert.That(foundPropertyType!.equals(_PrimitiveTypes.TheOne.__String));

            var dynamicRuntimeProvider = dm.WorkspaceLogic.GetTypesWorkspace()
                    .Resolve(_DatenMeister.TheOne.DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig.GetUri()!,
                        ResolveType.Default)
                as IElement;
            var foundPropertyType2 = ClassifierMethods.GetPropertyType(dynamicRuntimeProvider,
                _DatenMeister._DynamicRuntimeProvider._DynamicRuntimeLoaderConfig.configuration);

            Assert.That(foundPropertyType2, Is.Null);
        }
    }
}