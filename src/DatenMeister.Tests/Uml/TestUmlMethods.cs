using System.Linq;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.DataLayer;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Uml.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestUmlMethods
    {
        [Test]
        public void TestGeneralizedProperties()
        {
            var kernel = new ContainerBuilder();
            kernel.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            var builder = kernel.UseDatenMeisterDotNet(new IntegrationSettings { PathToXmiFiles = "Xmi" });
            using (var scope = builder.BeginLifetimeScope())
            {
                var classifierMethods = scope.Resolve<ClassifierMethods>();
                classifierMethods.Legacy = false;
                var dataLayers = scope.Resolve<DataLayers>();
                var dataLayerLogic = scope.Resolve<DataLayerLogic>();

                // Gets the logic
                var uml = dataLayerLogic.Get<_UML>(dataLayers.Uml);
                var feature = uml.Classification.__Feature;
                var properties = classifierMethods.GetPropertiesOfClassifier(feature).ToList();

                Assert.That(properties.Contains(_UML._Classification._Feature.isStatic), Is.True, 
                    "isStatic");
                Assert.That(properties.Contains(_UML._Classification._RedefinableElement.isLeaf), Is.True,
                    "isLeaf (Parent)");
                Assert.That(properties.Contains(_UML._CommonStructure._NamedElement.name), Is.True,
                    "name (Parent of Parent)");
            }
        }
    }
}