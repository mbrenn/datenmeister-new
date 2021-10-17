using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager;
using DatenMeister.Integration;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestClassifierMethods
    {
        [Test]
        public void TestCompositeProperties()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var classifier = dm.WorkspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.Classification.__Classifier.GetUri()!, ResolveType.Default)
                as IElement;
            Assert.That(classifier, Is.Not.Null);

            var properties = ClassifierMethods.GetCompositingProperties(classifier).ToList();
            Assert.That(properties, Is.Not.Null);
            var propertyList = properties.ToList();
            Assert.That(propertyList.Count(), Is.GreaterThan(0));
            
            var allProperties = ClassifierMethods.GetPropertiesOfClassifier(classifier).ToList();
            Assert.That(allProperties, Is.Not.Null);
            var allPropertyList = allProperties.ToList();
            Assert.That(allPropertyList.Count, Is.GreaterThan(propertyList.Count));
        }
        

        [Test]
        public void TestAddGeneralization()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var classifier = dm.WorkspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.Classification.__Classifier.GetUri()!, ResolveType.Default)
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
        public void TestGeneralizedProperties()
        {
            using var builder = DatenMeisterTests.GetDatenMeisterScope();
            using var scope = builder.BeginLifetimeScope();

            // Gets the logic
            var feature = builder.WorkspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.Classification.__Feature.GetUri()!, ResolveType.Default)
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
        public void TestGeneralizationEvaluation()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            
            var classifier = dm.WorkspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.Classification.__Classifier.GetUri()!, ResolveType.Default)
                as IElement;
            var class2 = dm.WorkspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.StructuredClassifiers.__Class.GetUri()!, ResolveType.Default)
                as IElement;
            var comment = dm.WorkspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.CommonStructure.__Comment.GetUri()!, ResolveType.Default)
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
        public void TestIsOfPrimitiveType()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            
            var activity = dm.WorkspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.Activities.__Activity.GetUri()!, ResolveType.Default)
                as IElement;
            var integer = dm.WorkspaceLogic.GetUmlWorkspace().Resolve(_PrimitiveTypes.TheOne.__Integer.GetUri()!, ResolveType.Default)
                as IElement;

            Assert.That(ClassifierMethods.IsOfPrimitiveType(activity!), Is.False);

            Assert.That(ClassifierMethods.IsOfPrimitiveType(integer!), Is.True);
        }
    }
}