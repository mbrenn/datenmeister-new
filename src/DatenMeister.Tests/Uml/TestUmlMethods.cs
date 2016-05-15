using System.Linq;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.DataLayer;
using DatenMeister.Integration;
using DatenMeister.Uml.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture()]
    public class TestUmlMethods
    {
        [Test]
        public void TestGeneralizedProperties()
        {
            var kernel = new ContainerBuilder();
            kernel.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            var builder = kernel.UseDatenMeister(new IntegrationSettings { PathToXmiFiles = "Xmi" });
            using (var scope = builder.BeginLifetimeScope())
            {
                var classifierMethods = scope.Resolve<ClassifierMethods>();
                classifierMethods.Legacy = true;
                var dataLayers = scope.Resolve<DataLayers>();
                var dataLayerLogic = scope.Resolve<DataLayerLogic>();

                // Gets the logic
                var uml = dataLayerLogic.Get<_UML>(dataLayers.Uml);
                var feature = uml.Classification.__Feature;
                var properties = classifierMethods.GetPropertiesOfClassifier(feature).ToList();

                Assert.That(properties.Contains(uml.Classification.Feature.isStatic), Is.True, "isStatic");
                Assert.That(properties.Contains(uml.Classification.RedefinableElement.isLeaf), Is.True,
                    "isLeaf (Parent)");
                Assert.That(properties.Contains(uml.CommonStructure.NamedElement.name), Is.True,
                    "name (Parent of Parent)");
            }
        }
    }
}