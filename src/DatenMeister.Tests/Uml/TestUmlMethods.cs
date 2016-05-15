using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.Full.Integration;
using DatenMeister.Uml.Helper;
using Ninject;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture()]
    public class TestUmlMethods
    {
        [Test]
        public void TestGeneralizedProperties()
        {
            var kernel = new StandardKernel();
            kernel.UseDatenMeister(new IntegrationSettings { PathToXmiFiles = "Xmi" });

            var classifierMethods = kernel.Get<ClassifierMethods>();
            classifierMethods.Legacy = true;
            var dataLayers = kernel.Get<DataLayers>();
            var dataLayerLogic = kernel.Get<DataLayerLogic>();

            // Gets the logic
            var uml = dataLayerLogic.Get<_UML>(dataLayers.Uml);
            var feature = uml.Classification.__Feature;
            var properties = classifierMethods.GetPropertiesOfClassifier(feature).ToList();

            Assert.That(properties.Contains(uml.Classification.Feature.isStatic), Is.True, "isStatic");
            Assert.That(properties.Contains(uml.Classification.RedefinableElement.isLeaf), Is.True, "isLeaf (Parent)");
            Assert.That(properties.Contains(uml.CommonStructure.NamedElement.name), Is.True, "name (Parent of Parent)");
        }
    }
}