using System.Linq;
using Autofac;
using Autofac.Features.ResolveAnything;
using DatenMeister.Core;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.Filler;
using DatenMeister.Integration;
using DatenMeister.Integration.DotNet;
using DatenMeister.Runtime.Workspaces;
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
            var builder = kernel.UseDatenMeisterDotNet(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                var classifierMethods = scope.Resolve<ClassifierMethods>();
                var dataLayers = scope.Resolve<DataLayers>();
                var dataLayerLogic = scope.Resolve<DataLayerLogic>();

                // Gets the logic
                var uml = dataLayerLogic.Get<_UML>(dataLayers.Uml);
                var feature = uml.Classification.__Feature;
                var properties = classifierMethods.GetPropertyNamesOfClassifier(feature).ToList();

                Assert.That(properties.Contains(_UML._Classification._Feature.isStatic), Is.True,
                    "isStatic");
                Assert.That(properties.Contains(_UML._Classification._RedefinableElement.isLeaf), Is.True,
                    "isLeaf (Parent)");
                Assert.That(properties.Contains(_UML._CommonStructure._NamedElement.name), Is.True,
                    "name (Parent of Parent)");
            }
        }

        [Test]
        public void TestFullName()
        {
            var kernel = new ContainerBuilder();
            kernel.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            var builder = kernel.UseDatenMeisterDotNet(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                var workspaceCollection = scope.Resolve<IWorkspaceCollection>();
                var dataLayers = scope.Resolve<DataLayers>();
                var dataLayerLogic = scope.Resolve<DataLayerLogic>();

                // Gets the logic
                var uml = dataLayerLogic.Get<_UML>(dataLayers.Uml);
                var feature = uml.Classification.__Feature;
                var namedElementMethods = scope.Resolve<NamedElementMethods>();
                var fullName = namedElementMethods.GetFullName(feature);

                Assert.That(fullName, Is.Not.Null);
                Assert.That(fullName, Is.EqualTo("UML::Classification::Feature"));
                
                var umlExtent = workspaceCollection.GetWorkspace(WorkspaceNames.Uml).FindExtent(Locations.UriUml);
                // now the other way
                var foundElement = namedElementMethods.GetByFullName(umlExtent.elements(), fullName);
                Assert.That(foundElement, Is.EqualTo(feature));
            }
        }
    }
}